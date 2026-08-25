using ATM_Simulator.Data;
using ATM_Simulator.Models;
using Microsoft.EntityFrameworkCore;

// ========================================
// ATM SIMULATOR
// ========================================

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("================================");
Console.WriteLine("         ATM SIMULATOR");
Console.WriteLine("================================");
Console.WriteLine();

// ========================================
// DATABASE
// ========================================

using ATMDbContext db = new ATMDbContext();

// ========================================
// FIND ACCOUNT
// ========================================

string accountNumber = "1234567890";

Account? account = db.Accounts
    .FirstOrDefault(a => a.AccountNumber == accountNumber);

if (account == null)
{
    Console.WriteLine("Account not found.");
    Console.WriteLine("Please contact your bank.");
    return;
}

// ========================================
// PIN LOGIN
// ========================================

int attempts = 0;
bool isAuthenticated = false;

while (attempts < 3 && !isAuthenticated)
{
    Console.Write("Enter your PIN: ");

    int enteredPin = Convert.ToInt32(Console.ReadLine());

    if (enteredPin == account.PIN)
    {
        isAuthenticated = true;

        Console.WriteLine();
        Console.WriteLine("PIN verified successfully.");
        Console.WriteLine("Welcome, " + account.AccountHolderName);
    }
    else
    {
        attempts++;

        Console.WriteLine("Invalid PIN.");
        Console.WriteLine("Attempts remaining: " + (3 - attempts));
        Console.WriteLine();
    }
}

// ========================================
// ACCOUNT LOCK CHECK
// ========================================

if (!isAuthenticated)
{
    Console.WriteLine("Your account has been locked.");
    Console.WriteLine("Please contact your bank.");
    return;
}

// ========================================
// ATM SETTINGS
// ========================================

decimal minimumBalance = 500;
decimal maximumWithdrawal = 10000;

// ========================================
// ATM MENU
// ========================================

int choice = 0;

while (choice != 7)
{
    Console.WriteLine();

    Console.WriteLine("================================");
    Console.WriteLine("         ATM MAIN MENU");
    Console.WriteLine("================================");

    Console.WriteLine("1. Check Balance");
    Console.WriteLine("2. Deposit Money");
    Console.WriteLine("3. Withdraw Money");
    Console.WriteLine("4. Account Information");
    Console.WriteLine("5. Change PIN");
    Console.WriteLine("6. Logout");
    Console.WriteLine("7. Exit");

    Console.WriteLine();

    Console.Write("Enter your choice: ");

    choice = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine();

    switch (choice)
    {
        // ====================================
        // CHECK BALANCE
        // ====================================

        case 1:

            Console.WriteLine("--------------------------------");
            Console.WriteLine("          ACCOUNT BALANCE");
            Console.WriteLine("--------------------------------");

            Console.WriteLine(
                "Current Balance: ₹" + account.Balance);

            break;


        // ====================================
        // DEPOSIT MONEY
        // ====================================

        case 2:

            Console.WriteLine("--------------------------------");
            Console.WriteLine("          DEPOSIT MONEY");
            Console.WriteLine("--------------------------------");

            Console.Write("Enter amount to deposit: ");

            decimal deposit = Convert.ToDecimal(
                Console.ReadLine());

            if (deposit <= 0)
            {
                Console.WriteLine("Invalid amount.");
                Console.WriteLine(
                    "Deposit amount must be greater than 0.");
            }
            else
            {
                account.Balance = account.Balance + deposit;

                Transaction transaction = new Transaction
                {
                    AccountId = account.AccountId,
                    TransactionType = "Deposit",
                    Amount = deposit,
                    TransactionDate = DateTime.Now,
                    BalanceAfterTransaction = account.Balance
                };

                db.Transactions.Add(transaction);

                db.SaveChanges();

                Console.WriteLine();
                Console.WriteLine(
                    "Amount deposited successfully.");

                Console.WriteLine(
                    "Deposited Amount: ₹" + deposit);

                Console.WriteLine(
                    "New Balance: ₹" + account.Balance);
            }

            break;


        // ====================================
        // WITHDRAW MONEY
        // ====================================

        case 3:

            Console.WriteLine("--------------------------------");
            Console.WriteLine("          WITHDRAW MONEY");
            Console.WriteLine("--------------------------------");

            Console.Write("Enter amount to withdraw: ");

            decimal withdraw = Convert.ToDecimal(
                Console.ReadLine());

            if (withdraw <= 0)
            {
                Console.WriteLine("Invalid amount.");
                Console.WriteLine(
                    "Withdrawal amount must be greater than 0.");
            }
            else if (withdraw > maximumWithdrawal)
            {
                Console.WriteLine(
                    "Withdrawal limit exceeded.");

                Console.WriteLine(
                    "Maximum withdrawal allowed: ₹"
                    + maximumWithdrawal);
            }
            else if (withdraw > account.Balance)
            {
                Console.WriteLine("Insufficient balance.");
            }
            else if (account.Balance - withdraw < minimumBalance)
            {
                Console.WriteLine(
                    "Transaction cannot be completed.");

                Console.WriteLine(
                    "You must maintain a minimum balance of ₹"
                    + minimumBalance);
            }
            else
            {
                account.Balance =
                    account.Balance - withdraw;

                Transaction transaction = new Transaction
                {
                    AccountId = account.AccountId,
                    TransactionType = "Withdrawal",
                    Amount = withdraw,
                    TransactionDate = DateTime.Now,
                    BalanceAfterTransaction = account.Balance
                };

                db.Transactions.Add(transaction);

                db.SaveChanges();

                Console.WriteLine();
                Console.WriteLine(
                    "Please collect your cash.");

                Console.WriteLine(
                    "Withdrawn Amount: ₹" + withdraw);

                Console.WriteLine(
                    "Remaining Balance: ₹"
                    + account.Balance);
            }

            break;


        // ====================================
        // ACCOUNT INFORMATION
        // ====================================

        case 4:

            Console.WriteLine("--------------------------------");
            Console.WriteLine("       ACCOUNT INFORMATION");
            Console.WriteLine("--------------------------------");

            Console.WriteLine(
                "Account Holder : "
                + account.AccountHolderName);

            Console.WriteLine(
                "Account Number : "
                + account.AccountNumber);

            Console.WriteLine(
                "Balance        : ₹"
                + account.Balance);

            break;


        // ====================================
        // CHANGE PIN
        // ====================================

        case 5:

            Console.WriteLine("--------------------------------");
            Console.WriteLine("            CHANGE PIN");
            Console.WriteLine("--------------------------------");

            Console.Write("Enter current PIN: ");

            int oldPin = Convert.ToInt32(
                Console.ReadLine());

            if (oldPin == account.PIN)
            {
                Console.Write("Enter new PIN: ");

                int newPin = Convert.ToInt32(
                    Console.ReadLine());

                if (newPin >= 1000 && newPin <= 9999)
                {
                    account.PIN = newPin;

                    db.SaveChanges();

                    Console.WriteLine(
                        "PIN changed successfully.");
                }
                else
                {
                    Console.WriteLine(
                        "PIN must contain exactly 4 digits.");
                }
            }
            else
            {
                Console.WriteLine(
                    "Incorrect current PIN.");
            }

            break;


        // ====================================
        // LOGOUT
        // ====================================

        case 6:

            Console.WriteLine(
                "You have been logged out.");

            Console.WriteLine(
                "Thank you for using our ATM.");

            choice = 7;

            break;


        // ====================================
        // EXIT
        // ====================================

        case 7:

            Console.WriteLine(
                "Thank you for using our ATM.");

            Console.WriteLine(
                "Have a nice day!");

            break;


        // ====================================
        // INVALID CHOICE
        // ====================================

        default:

            Console.WriteLine("Invalid choice.");

            Console.WriteLine(
                "Please select a number between 1 and 7.");

            break;
    }
}