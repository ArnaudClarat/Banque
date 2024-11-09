Person arnaud = new("Arnaud", "Clarat", new DateTime(1997,7,14));
CurrentAccount account = new("BE68 5390 0754 7034", 157.25, 100.00, arnaud);
Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
account.Withdraw(3.65);
Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
account.Withdraw(251.32);
Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
account.ApplyInterests();
Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
account.Deposit(1.78);
Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
account.Deposit(3254.56);
Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
account.ApplyInterests();
Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");


public class Person(string firstName, string lastName, DateTime birthDate)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public DateTime BirthDate { get; set; } = birthDate;
}

public interface IAccount
{
    double Balance { get; }
    void Deposit(double amount);
    void Withdraw(double amount);
}

public interface IBankAccount : IAccount
{
    void ApplyInterests();
    Person Owner { get; }
    string Number { get; }
}

abstract class Account : IBankAccount
{
    public string Number { get; protected set; }
    public double Balance { get; protected set; }
    public double CreditLine { get; protected set; }
    public Person Owner { get; protected set; }
    protected Account(string number, double balance, double creditLine, Person owner)
    {
        Number = number;
        Balance = balance;
        CreditLine = creditLine;
        Owner = owner;
    }
    public abstract void Withdraw(double amount);
    public void Deposit(double amount)
    {
        Balance += amount;
        Console.WriteLine($"{amount}€ has been deposited successfully");
        if (Balance < 0)
        {
            Console.WriteLine("Warning, your balance is negative.");
        }
    }
    public void ApplyInterests()
    {
        Balance += CalculInterests();
    }
    public abstract double CalculInterests();
    
}

class CurrentAccount : Account 
{
    public CurrentAccount(string number, double balance, double creditLine, Person owner) : base(number, balance, creditLine, owner) {}
    public override void Withdraw(double amount)
    {
        if (Balance - amount >= -CreditLine)
        {
            Balance -= amount;
            Console.WriteLine($"{amount}€ has been withdrawed successfully.");
            if (Balance < 0)
            {
                Console.WriteLine("Warning, your balance is negative.");
            }
        }
        else
        {
            Console.WriteLine($"Insuficent fonds to withdraw {amount}€");
        }
    }
    public override double CalculInterests()
    {
        Console.WriteLine("Calcul des interets...");
        return Balance >= 0 ? Balance * 0.03 : Balance * 0.0975;
    }
}

class SavingsAccount : Account
{
    public SavingsAccount(string number, double balance, double creditLine, Person owner) : base(number, balance, creditLine, owner) { }
    public DateTime DateLastWithdraw { get; set; }
    public override void Withdraw(double amount)
    {
        if (Balance - amount >= -CreditLine)
        {
            Balance -= amount;
            Console.WriteLine($"{amount}€ has been withdrawed successfully.");
            if (Balance < 0)
            {
                Console.WriteLine("Warning, your balance is negative.");
            }
        }
        else
        {
            Console.WriteLine($"Insuficent fonds to withdraw {amount}€");
        }
    }
    public override double CalculInterests()
    {
        Console.WriteLine("Calcul des interets...");
        return Balance * 0.0045;
    }
}