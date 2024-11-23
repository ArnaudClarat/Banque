try
{
    Person arnaud = new("Arnaud", "Clarat", new DateTime(1997, 7, 14));
    CurrentAccount account = new("BE68 5390 0754 7034", 157.25, 1000, arnaud);
    account.NegativeBalanceEvent += sender => Console.WriteLine($"Negative balance alert for account: {(sender as CurrentAccount)?.Number}");
    Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
    account.Withdraw(3.65);
    Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
    account.Withdraw(251.32);
    Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
    account.ApplyInterests();
    Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
    account.Deposit(-1.78);
    Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
    account.Deposit(3254.56);
    Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
    account.ApplyInterests();
    Console.WriteLine($"\t\tbalance : {account.Balance:F2}€");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}



public class Person(string firstName, string lastName, DateTime birthDate)
{
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
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
    public string Number { get; private set; }
    public double Balance { get; private set; }
    public double CreditLine { get; private set; }
    public Person Owner { get; private set; }
    //public delegate void NegativeBalanceDelegate(Account account);
    public event Action<Account>? NegativeBalanceEvent;
    protected Account(string number, double balance, double creditLine, Person owner)
    {
        if (creditLine < 0)
        {
            throw new ArgumentOutOfRangeException(null, "CreditLine must be greater than or equal to 0."); 
        }
        Number = number;
        Balance = balance;
        CreditLine = creditLine;
        Owner = owner;
    }
    protected Account(string number, Person owner):this(number, 0, 0, owner) { }
    protected Account(string number, double balance, Person owner):this(number, balance, 0, owner) { }
    public virtual void Withdraw(double amount)
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
            throw new InsuficientBalanceException($"Insuficent fonds to withdraw {amount}€");
        }
    }
    public void Deposit(double amount)
    {
        if (amount > 0)
        {
            Balance += amount;
            Console.WriteLine($"{amount}€ has been deposited successfully");
            if (Balance < 0)
            {
                Console.WriteLine("Warning, your balance is negative.");
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(null, "Veuillez inserez un montant positif");
        }
    }
    public void ApplyInterests()
    {
        Balance += CalculInterests();
    }
    public abstract double CalculInterests();
    protected void OnNegativeBalance()
    {
        NegativeBalanceEvent?.Invoke(this);
    }

}

class CurrentAccount : Account 
{
    public CurrentAccount(string number, double balance, double creditLine, Person owner) : base(number, balance, creditLine, owner) {}
    public CurrentAccount(string number, double balance, Person owner) : base(number, balance, 0, owner) {}
    public CurrentAccount(string number, Person owner) : base(number, 0, 100, owner) {}
    public override void Withdraw(double amount)
    {
        if (Balance > 0 && Balance - amount < 0)
        {
            OnNegativeBalance();
        }
        base.Withdraw(amount);
    }
    public override double CalculInterests()
    {
        Console.WriteLine("Calcul des interets...");
        return Balance >= 0 ? Balance * 0.03 : Balance * 0.0975;
    }

}

class SavingsAccount : Account
{
    public SavingsAccount(string number, double balance, Person owner) : base(number, balance, 0, owner) {}
    public SavingsAccount(string number, Person owner) : base(number, 0, 0, owner) {}
    public DateTime DateLastWithdraw { get; private set; }
    public override double CalculInterests()
    {
        Console.WriteLine("Calcul des interets...");
        return Balance * 0.0045;
    }
}

class InsuficientBalanceException : Exception 
{
    public InsuficientBalanceException(string message) : base(message) { }
}