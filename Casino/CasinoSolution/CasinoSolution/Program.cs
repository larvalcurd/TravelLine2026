const int MinRandom = 1;
const int MaxRandom = 20;
const int WinFrom = 18;
const int Multiplier = 2;

PrintTitle();

int balance = ReadPositiveInt( "Введите начальный баланс: " );
var random = new Random();

RunGameLoop( balance, random );

return;

static void RunGameLoop( int startBalance, Random random )
{
    int balance = startBalance;

    while ( true )
    {
        PrintMenu();
        Console.Write( "Выберите действие: " );
        string choice = Console.ReadLine()?.Trim();

        switch ( choice )
        {
            case "1":
                Console.WriteLine( $"Текущий баланс: {balance}" );
                break;

            case "2":
                if ( balance <= 0 )
                {
                    Console.WriteLine( "Баланс равен 0. Игра завершена." );
                    return;
                }

                int bet = ReadBet( balance );
                int randomNumber = GenerateRandomNumber( random );
                RoundResult roundResult = PlayRound( balance, bet, randomNumber );

                PrintRoundResult( roundResult );
                balance = roundResult.BalanceAfter;

                if ( balance <= 0 )
                {
                    Console.WriteLine( "Баланс закончился. Игра завершена." );
                    return;
                }

                break;

            case "3":
                Console.WriteLine( "Спасибо за игру!" );
                return;

            default:
                Console.WriteLine( "Некорректный выбор. Введите 1, 2 или 3." );
                break;
        }
    }
}

static int GenerateRandomNumber( Random random )
{
    return random.Next( MinRandom, MaxRandom + 1 );
}

static bool IsWinningNumber( int randomNumber )
{
    return randomNumber >= WinFrom && randomNumber <= MaxRandom;
}

static int CalculatePayout( int bet, int randomNumber )
{
    if ( !IsWinningNumber( randomNumber ) )
    {
        return 0;
    }
    return bet * ( 1 + ( ( Multiplier * randomNumber ) % 17 ) );
}

static RoundResult PlayRound( int balance, int bet, int randomNumber )
{
    int payout = CalculatePayout( bet, randomNumber );
    bool isWin = payout > 0;
    int balanceAfter = balance - bet + payout;

    return new RoundResult(
        Bet: bet,
        RandomNumber: randomNumber,
        IsWin: isWin,
        Payout: payout,
        BalanceBefore: balance,
        BalanceAfter: balanceAfter
    );
}

static void PrintRoundResult( RoundResult result )
{
    Console.WriteLine( $"Выпало число: {result.RandomNumber}" );

    if ( result.IsWin )
    {
        Console.WriteLine( $"Выигрыш! Начислено: {result.Payout}" );
    }
    else
    {
        Console.WriteLine( "Проигрыш." );
    }

    Console.WriteLine( $"Ставка: {result.Bet}" );
    Console.WriteLine( $"Баланс до раунда: {result.BalanceBefore}" );
    Console.WriteLine( $"Баланс после раунда: {result.BalanceAfter}" );
}

static int ReadBet( int balance )
{
    while ( true )
    {
        Console.Write( "Введите ставку: " );
        string input = Console.ReadLine();

        if ( !int.TryParse( input, out int bet ) )
        {
            Console.WriteLine( "Введите корректное число." );
            continue;
        }

        if ( bet <= 0 )
        {
            Console.WriteLine( "Ставка должна быть больше 0." );
            continue;
        }

        if ( bet > balance )
        {
            Console.WriteLine( "Ставка не может быть больше текущего баланса." );
            continue;
        }

        return bet;
    }
}

static int ReadPositiveInt( string prompt )
{
    while ( true )
    {
        Console.Write( prompt );
        string input = Console.ReadLine();

        if ( !int.TryParse( input, out int value ) )
        {
            Console.WriteLine( "Введите корректное число." );
            continue;
        }

        if ( value <= 0 )
        {
            Console.WriteLine( "Число должно быть больше 0." );
            continue;
        }

        return value;
    }
}

static void PrintTitle()
{
    Console.WriteLine( "###############################" );
    Console.WriteLine( "########## CASINO #############" );
    Console.WriteLine( "###############################" );
}

static void PrintMenu()
{
    Console.WriteLine();
    Console.WriteLine( "1. Показать текущий баланс" );
    Console.WriteLine( "2. Сыграть" );
    Console.WriteLine( "3. Выйти" );
}

public record RoundResult(
    int Bet,
    int RandomNumber,
    bool IsWin,
    int Payout,
    int BalanceBefore,
    int BalanceAfter
);
