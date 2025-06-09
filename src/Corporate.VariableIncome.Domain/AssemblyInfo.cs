using System.Runtime.CompilerServices;

/*
 * Comentário de Intenção:
 * 
 * Habilitar que a camada de Infrastrutura e Testes Unitários visualize os métodos internos de criação de objeto de valor.
 */
[assembly: InternalsVisibleTo("Corporate.VariableIncome.Infrascructure")]
[assembly: InternalsVisibleTo("Corporate.VariableIncome.UnitTests")]