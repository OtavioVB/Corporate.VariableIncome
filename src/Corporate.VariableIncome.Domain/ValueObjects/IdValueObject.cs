using Corporate.VariableIncome.Domain.Exceptions;

namespace Corporate.VariableIncome.Domain.ValueObjects;

/*
 * Comentário de Intenção:
 * 
 * Objetos de valor são abstrações criadas nas quais encapsulam regras de negócio de valores de atributos de domínio. A implementação
 * nesse formato é intencional e motivada de forma a garantir que o atributo persistido ao longo do processamento dentro do domínio
 * só seja obtido quando seu estado for válido, haja visto que se IdValueObject for criado como default em memória, o atributo IsValid será
 * false (valor padrão para o tipo primitivo booleano) e value será inválid (como nulo). 
 * 
 * Seu modificador de acesso foi classificado como 'public readonly', pois quero garantir que esse objeto de valor não sofra mutações
 * ao longo de sua vida em memória e possa ser persistido durante todo o ciclo de processamento sem alterações de estado (Imutabilidade)
 */
public readonly struct IdValueObject
{
    public bool IsValid { get; }
    private Guid? Value { get; }

    /*
    * Comentário de Intenção:
    * 
    * Os métodos construtores são utilizados para instanciação dos objetos/structs em memória, apenas, por definição essa é sua responsabilidade.
    * Ao contrário do que muitos desenvolvedores fazem, esse método foi modificado seu acesso como privado, pois quero garantir que o objeto só
    * seja instanciado por uma regra de negócio prévia relacionado a aquele atributo, não é atoa que o método "Build" foi criado seguindo algumas
    * regras de validação.
    */
    private IdValueObject(bool isValid, Guid? value)
    {
        IsValid = isValid;
        Value = value;
    }

    /*
     * Comentário de Intenção:
     * 
     * Em diversos objetos fora do domínio como ViewModels,
     * DataModels, DTOs em geral, é necessário referenciar o
     * tamanho do campo, principalmente na configuração de
     * ORMs, por isso, é importante que todos tenham a mesma
     * referência para o tamanho do campo para evitar duplicidade
     * e problemas de manutenção com relação a isso. Seria um
     * grande problema no domínio ter um tamanho e na ViewModel
     * do Asp.Net mvc ter outro, por exemplo.
     * 
     * Por isso, esses metadados são expostos aqui por meio
     * de constantes
     */
    public const int LENGTH = 36; // Equivalente a Guid.NewGuid().ToString().Length

    /*
     * Comentário de Intenção:
     * 
     * O método "UnsafeBuildFromTrustedDatabaseRecord" tem como objetivo construir um objeto de valor do tipo "IdValueObject"
     * a partir de um valor primitivo previamente persistido no banco de dados e considerado como confiável.
     * 
     * Esse método é classificado como "internal", pois sua intenção é ser utilizado exclusivamente por mecanismos internos de infraestrutura,
     * como os mapeamentos do Entity Framework Core, durante o processo de hidratação de entidades em memória.
     * 
     * A decisão de não realizar validações de negócio dentro desse método é intencional, pois assume-se que o valor armazenado no banco
     * já passou anteriormente por um processo de validação e, portanto, é confiável para ser reconstruído como objeto de valor diretamente.
     * 
     * Essa estratégia é adotada para garantir desempenho na leitura de dados, especialmente em contextos de APIs com alto volume de requisições,
     * evitando o custo computacional desnecessário de validações repetidas em tempo de leitura.
     * 
     * O nome do método foi prefixado com "Unsafe" para reforçar que seu uso deve ser restrito e que qualquer dado de entrada que não seja
     * originado de uma fonte já validada (como o banco de dados) pode comprometer a integridade do domínio.
     * 
     * Importante: o uso desse método fora da camada de infraestrutura pode gerar inconsistências e violar as regras de negócio do domínio,
     * portanto, **nunca** deve ser utilizado em camadas como aplicação, serviços de entrada (APIs) ou comandos de escrita.
     */
    internal static IdValueObject UnsafeBuildFromTrustedDatabaseRecord(Guid input)
        => new IdValueObject(
            isValid: true,
            value: input);

    /*
     * Comentário de Intenção:
     * 
     * O método Build é responsável pela validação e garantia da regra de negócio sobre o objeto de valor de domínio de negócio.
     * 
     * Além disso, como a construção/instãnciação em memória é declarada como privada, fazemos com que esse objeto de valor seja imutável
     * o que corrobora para a garantia de seu estado no seu ciclo de vida durante a requisição de um processamento.
     * 
     * Nesse caso, a regra de negócio executada por "IdValueObject" consiste na criação de um Id válido caso esse valor seja enviado
     * como nulo, e caso esse valor não seja enviado como nulo, é necessário garantir que ele é válido (ou seja, qualquer id que não seja empty).
     */
    public static IdValueObject Build(Guid? value = null)
    {
        if (value == null)
            return new IdValueObject(
                isValid: true,
                value: Guid.NewGuid());

        var requiredValue = value!.Value;

        if (requiredValue == Guid.Empty)
            return new IdValueObject(
                isValid: false,
                value: null);


        return new IdValueObject(
            isValid: true,
            value: value);
    }

    /*
     * Comentário de Intenção:
     * 
     * Esse método foi criado intencionalmente dessa maneira, pois objetos de valor devem garantir regra de negócio, e atributos de regra
     * de negócio como Id carregam alguma regra, nas quais fazem com que seu tipo primitivo (Guid) só seja obtido caso seu estado seja válido.
     */ 
    public Guid GetValue()
    {
        ValueObjectException.ThrowValueObjectExceptionWhenResourceIsNotValid<IdValueObject>(IsValid);

        return Value!.Value;
    }

    /*
     * Os métodos a seguir foram criados como forma de auxiliar o desenvolvedor com ferramentas para facilitar mappings entre outras coisas.
     */ 
    public string GetValueAsString()
        => GetValue().ToString();

    public static implicit operator IdValueObject(Guid obj)
        => Build(obj);
    public static implicit operator Guid(IdValueObject obj)
        => obj.GetValue();
}
