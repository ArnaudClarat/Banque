Person arnaud = new("Arnaud", "Clarat", new DateTime(1997,7,14));
CurrentAccount account = new("BE68 5390 0754 7034", 157.25, 100.00, arnaud);
Console.WriteLine("balance : " + account.Balance);
account.Withdraw(3.65);
Console.WriteLine("balance : " + account.Balance);
account.Withdraw(254.32);
Console.WriteLine("balance : " + account.Balance);
account.Deposit(1.78);
Console.WriteLine("balance : " + account.Balance);
account.Deposit(3254.56);
Console.WriteLine("balance : " + account.Balance);

class Person(string firstName, string lastName, DateTime birthDate)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public DateTime BirthDate { get; set; } = birthDate;
}

class CurrentAccount(string number, double balance, double creditLine, Person owner)
{
    public string Number { get; set; } = number;
    public double Balance { get; private set; } = balance;
    public double CreditLine { get; set; } = creditLine;
    public Person Owner { get; set; } = owner;

    public void Withdraw(double amount)
    {
        if ((amount < Balance) || (Balance - amount < CreditLine))
        {
            Balance -= amount;
            Console.WriteLine($"{amount} has been withdrawed successfully.");
            if (Balance < 0)
            {
                Console.WriteLine("Warning, your balance is negative.");
            }
        }
        else
        {
            Console.WriteLine($"Insuficent fonds to withdraw {amount}");
        }
    }

    public void Deposit(double amount)
    {
        Balance += amount;
        Console.WriteLine($"{amount} has been deposited successfully");
        if (Balance < 0)
        {
            Console.WriteLine("Warning, your balance is negative.");
        }
    }
}