using System;
using System.Collections.Generic;

public interface ITarifa
{
    double CalcularTarifa();
}

public abstract class Conta
{
    public double SaldoEmReais { get; protected set; }

    public Conta(double saldoEmReais)
    {
        SaldoEmReais = saldoEmReais;
    }

    public abstract double CalcularSaldoEmReais();
}

public class ContaCorrente : Conta, ITarifa
{
    public ContaCorrente(double saldoEmReais) : base(saldoEmReais) { }

    public override double CalcularSaldoEmReais() => SaldoEmReais;

    public double CalcularTarifa() => SaldoEmReais * 0.015;
}

public class ContaInternacional : Conta, ITarifa
{
    public double TaxaDeCambio { get; private set; }

    public ContaInternacional(double saldoEmDolares, double taxaDeCambio) : base(saldoEmDolares)
    {
        TaxaDeCambio = taxaDeCambio;
    }

    public override double CalcularSaldoEmReais() => SaldoEmReais * TaxaDeCambio;

    public double CalcularTarifa() => SaldoEmReais * 0.025;
}

public class ContaCripto : Conta
{
    public ContaCripto(double saldoEmDolares) : base(saldoEmDolares) { }

    public override double CalcularSaldoEmReais()
    {
        double taxaDeCambioCripto = 5.0;
        return SaldoEmReais * taxaDeCambioCripto;
    }
}

public class SistemaDeTarifas
{
    public double TotalSaldo { get; private set; }
    public double TotalTarifa { get; private set; }

    public void AcumularConta(Conta conta)
    {
        TotalSaldo += conta.CalcularSaldoEmReais();

        if (conta is ITarifa contaTarifada)
        {
            TotalTarifa += contaTarifada.CalcularTarifa();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        SistemaDeTarifas sistemaDeTarifas = new SistemaDeTarifas();
        List<Conta> contas = new List<Conta>();

        while (true)
        {
            Console.WriteLine("Escolha o tipo de conta:");
            Console.WriteLine("1 - Conta Corrente");
            Console.WriteLine("2 - Conta Internacional");
            Console.WriteLine("3 - Conta Cripto");
            Console.WriteLine("4 - Calcular e exibir resultados");
            Console.WriteLine("5 - Sair do programa");
            Console.Write("Opção: ");

            int escolha = Convert.ToInt32(Console.ReadLine());

            if (escolha == 5)
            {
                break;
            }

            Conta conta = null;

            switch (escolha)
            {
                case 1:
                    Console.Write("Digite o saldo em reais da Conta Corrente: ");
                    double saldoCorrente = Convert.ToDouble(Console.ReadLine());
                    conta = new ContaCorrente(saldoCorrente);
                    break;

                case 2:
                    Console.Write("Digite o saldo em dólares da Conta Internacional: ");
                    double saldoInternacional = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Digite a taxa de câmbio: ");
                    double taxaDeCambio = Convert.ToDouble(Console.ReadLine());
                    conta = new ContaInternacional(saldoInternacional, taxaDeCambio);
                    break;

                case 3:
                    Console.Write("Digite o saldo em dólares da Conta Cripto: ");
                    double saldoCripto = Convert.ToDouble(Console.ReadLine());
                    conta = new ContaCripto(saldoCripto);
                    break;

                case 4:
                    foreach (Conta c in contas)
                    {
                        sistemaDeTarifas.AcumularConta(c);
                    }
                    Console.WriteLine($"Valor total do saldo em reais: R${sistemaDeTarifas.TotalSaldo:F2}");
                    Console.WriteLine($"Valor total da tarifa em reais: R${sistemaDeTarifas.TotalTarifa:F2}");
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            if (conta != null)
            {
                contas.Add(conta);
                Console.WriteLine("Conta acumulada com sucesso.");
            }
        }
    }
}
