using System;

namespace BankApplication
{
    /// <summary>
    /// Base class to store the current balance and handle debit and credit transactions
    /// </summary>
    public class Account
    {
        decimal balance;

        public decimal Balance
        {
            get { return balance; }
            set
            {
                if (value >= 0.0M)
                {
                    balance = value;
                }
                else
                {
                    balance = 0.0M;
                    Console.WriteLine("Initial balance was invalid. Value has been replaced with $0");
                }
            }
        }

        public Account(decimal balance)
        {
            Balance = balance;
        }

        public virtual void Credit(decimal amount)
        {
            balance += amount;
        }

        public virtual bool Debit(decimal amount)
        {
            if (amount <= balance)
            {
                balance -= amount;
                return true;
            }
            else
            {
                Console.WriteLine("Debit amount exceeded account balance.");
                return false;
            }
        }

    }

    /// <summary>
    /// Savings account that allows for deposited money to earn interest
    /// inherits from base class
    /// </summary>
    public class SavingsAccount : Account
    {
        decimal interestRate;

        public SavingsAccount(decimal balance, decimal interestRate) : base(balance)
        {
            this.interestRate = interestRate;
        }

        public decimal CalculateInterest()
        {
            return Balance * interestRate / 100m;
        }
    }


    /// <summary>
    /// Checking account that allows for withdrawals at a set fee per transaction
    /// inherits from base class
    /// </summary>
    public class CheckingAccount : Account
    {
        decimal fee;

        public CheckingAccount(decimal balance, decimal fee) : base(balance)
        {
            this.fee = fee;
        }

        public override void Credit(decimal amount)
        {
            base.Credit(amount);
            if (amount > 0M) //There needs to be a change of funds for a fee
            {
                Balance -= fee; //remove fee for transaction
            }

        }

        public override bool Debit(decimal amount)
        {
            if (base.Debit(amount))
            {
                if (amount > 0M) //There needs to be a change of funds for a fee
                {
                    Balance -= fee; //remove fee for transaction
                }
                return true;
            }
            else { return false; }
        }

    }

    /// <summary>
    /// Test for polymorphism with the classes
    /// </summary>
    public class AccountTest
    {
        public static void Main(string[] args)
        {
            // create array of accounts
            Account[] accounts = new Account[4];

            // initialize array with Accounts
            accounts[0] = new SavingsAccount(25M, .03M);
            accounts[1] = new CheckingAccount(80M, 1M);
            accounts[2] = new SavingsAccount(200M, .015M);
            accounts[3] = new CheckingAccount(400M, .5M);

            // loop through array, prompting user for debit and credit amounts
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i] is SavingsAccount)
                {
                    Console.Write($"Savings ");
                }
                else
                {
                    Console.Write($"Checking ");
                }
                Console.WriteLine($"Account {i + 1} balance: {accounts[i].Balance:C}");

                Console.Write($"\nEnter an amount to withdraw from Account {i + 1}: ");
                decimal withdrawalAmount = decimal.Parse(Console.ReadLine());

                accounts[i].Debit(withdrawalAmount); // Attempt to debit account

                Console.Write($"\nEnter an amount to deposit into Account {i + 1}: ");
                decimal depositAmount = decimal.Parse(Console.ReadLine());

                // credit amount to Account
                accounts[i].Credit(depositAmount);

                // if Account is a SavingsAccount, calculate and add interest
                if (accounts[i] is SavingsAccount)
                {
                    // downcast
                    SavingsAccount currentAccount = (SavingsAccount)accounts[i];

                    decimal interestEarned = currentAccount.CalculateInterest();
                    Console.WriteLine($"Adding {interestEarned:C} interest to Account {i + 1} (a SavingsAccount)");
                    currentAccount.Credit(interestEarned);
                }

                Console.WriteLine($"\nUpdated Account {i + 1} balance: {accounts[i].Balance:C}\n\n");
            }

            Console.ReadKey();
        }
    }

  
}
