using Corporate.VariableIncome.Domain.BoundedContexts.AssetContext.Entities;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Enumerators;
using Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.ValueObjects;
using Corporate.VariableIncome.Domain.BoundedContexts.UserContext.Entities;
using Corporate.VariableIncome.Domain.Helpers;
using Corporate.VariableIncome.Domain.ValueObjects;

namespace Corporate.VariableIncome.Domain.BoundedContexts.OperationContext.Entities;

public sealed class Operation
{
    public Operation(
        IdValueObject id, 
        PriceValueObject unitaryPrice, 
        QuantityValueObject quantity, 
        BrokerageFeeValueObject brokerageFee, 
        DateTimeValueObject dateTime,
        TypeOperationValueObject type)
    {
        Id = id;
        UnitaryPrice = unitaryPrice;
        Quantity = quantity;
        BrokerageFee = brokerageFee;
        DateTime = dateTime;
        Type = type;
    }

    public IdValueObject Id { get; set; }
    public PriceValueObject UnitaryPrice { get; set; }
    public QuantityValueObject Quantity { get; set; }
    public BrokerageFeeValueObject BrokerageFee { get; set; }
    public IdValueObject AssetId { get; set; }
    public IdValueObject UserId { get; set; }
    public DateTimeValueObject DateTime { get; set; }
    public TypeOperationValueObject Type { get; set; }

    public bool IsValid => Id.IsValid && UnitaryPrice.IsValid && Quantity.IsValid && BrokerageFee.IsValid && AssetId.IsValid && DateTime.IsValid;

    public Asset? Relationship1AssetNOperations { get; set; }
    public User? Relationship1UserNOperations { get; set; }

    /*
     * Comentário de Intenção:
     * 
     * Esse é uma operação de negócio de cálculo de preço médio sobre todas as operações de compra de um ativo financeiro. Nele, seu resultado
     * de operação é encapsulado dentro de um objeto result, esse objeto Result é responsável por encapsular/orquestrar respostas de erro, e sucesso da operação.
     * Sendo a resposta de sucesso da operação o preço médio calculado, e a resposta de erro, uma mensagem de erro informando o motivo.
     * 
     * Nesse método de cálculo de preço médio de compra de um ativo financeiro ocorre uma série de validações:
     * 1. Se foi informado uma lista não vazia de operações de compra de ativo financeiro;
     * 2. Se todos os itens da lista são operações de compra de ativo financeiro.
     * 3. Se a quantidade informada na compra de um ativo financeiro for maior que 0.
     * 4. Se os atributos presente dentro da compra de um ativo financeiro é válido (preço unitário maior que 0,01, taxa de corretagem maior que 0 e menor que 1, entre outros);
     */
    public static Result<PriceValueObject> CalculateAveragePrice(Operation[] buyOperations)
    {
        if (buyOperations.Length == 0)
            return Result<PriceValueObject>.Error(
                "Para calcular preço médio de operações de compra de ativo financeiro é necessário enviar mais de uma operação.");

        decimal price = 0;
        int quantityTotal = 0;

        foreach (Operation operation in buyOperations)
        {
            if (!operation.IsValid)
                return Result<PriceValueObject>.Error("É necessário que todos os atributos de uma operação de compra de ativo financeiro.");

            if (operation.Type.GetValue() != EnumTypeOperation.BUY)
                return Result<PriceValueObject>.Error("As operações enviadas precisam ser operações de compra para cálcular preço médio.");

            if (operation.Quantity.GetValue() < 1)
                return Result<PriceValueObject>.Error("As operações de compra de ativo financeiro precisa conter quantidade maior que 0.");

            quantityTotal = operation.Quantity.GetValue() + quantityTotal;
            price = (operation.Quantity.GetValue() * operation.UnitaryPrice.GetValue()) + price;
        }

        decimal averagePrice = price / quantityTotal;

        return Result<PriceValueObject>.Success(averagePrice);
    }
}
