using System;
using System.Collections.Generic;
using System.Threading;

abstract class Enemy
{
    public int Level { get; set; }
    public int Health { get; set; }
    public int AttackDamage { get; set; }

    public Enemy(int level, int healthMultiplier, int damageMultiplier)
    {
        Level = level;
        Health = level * healthMultiplier;
        AttackDamage = level * damageMultiplier;
    }

    public abstract void DisplayInfo();
}

class NormalEnemy : Enemy
{
    public NormalEnemy(int level) : base(level, 5, 2)
    {
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Обычный враг уровня {Level} (Здоровье: {Health}, Урон: {AttackDamage})");
    }
}

class MiniBossEnemy : Enemy
{
    public MiniBossEnemy(int level) : base(level, 10, 5)
    {
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Мини-босс уровня {Level} (Здоровье: {Health}, Урон: {AttackDamage})");
    }
}

class BossEnemy : Enemy
{
    public BossEnemy(int level) : base(level, 15, 8)
    {
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Босс уровня {Level} (Здоровье: {Health}, Урон: {AttackDamage})");
    }
}

class Character
{
    public int Level { get; set; }
    public int Health { get; set; }
    public int AttackDamage { get; set; }

    public Character(int level, int health, int attackDamage)
    {
        Level = level;
        Health = health;
        AttackDamage = attackDamage;
    }

    public void LevelUp()
    {
        Level++;
        Health += 10;
        AttackDamage += 5;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Добро пожаловать в улучшенную консольную игру!");

        Character player = new Character(1, 30, 10);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Информация о персонаже:");
            Console.WriteLine($"Уровень: {player.Level}");
            Console.WriteLine($"Здоровье: {player.Health}");
            Console.WriteLine($"Урон: {player.AttackDamage}");
            Console.WriteLine();

            Console.WriteLine("Список врагов:");
            List<Enemy> enemies = GenerateEnemies(player.Level, 5);
            DisplayEnemyList(enemies);

            Console.Write("Выберите врага для боя (введите номер): ");
            if (int.TryParse(Console.ReadLine(), out int enemyIndex) && enemyIndex >= 0 && enemyIndex < enemies.Count)
            {
                Enemy selectedEnemy = enemies[enemyIndex];

                Console.WriteLine($"Вы выбрали врага уровня {selectedEnemy.Level}!");

                // Имитация боя с задержкой
                Console.WriteLine("Бой начался!");
                while (player.Health > 0 && selectedEnemy.Health > 0)
                {
                    Console.WriteLine($"Ваше здоровье: {player.Health}");
                    Console.WriteLine($"Здоровье врага: {selectedEnemy.Health}");

                    // Ход игрока
                    int playerDamage = CalculateDamage(player.AttackDamage);
                    selectedEnemy.Health -= playerDamage;
                    Console.WriteLine($"Вы атакуете врага и наносите {playerDamage} урона.");

                    Thread.Sleep(1000); // Задержка в 1 секунду

                    if (selectedEnemy.Health <= 0)
                    {
                        Console.WriteLine($"Вы победили врага!");
                        player.LevelUp();
                        break;
                    }

                    // Ход врага
                    int enemyDamage = CalculateDamage(selectedEnemy.AttackDamage);
                    player.Health -= enemyDamage;
                    Console.WriteLine($"Враг атакует вас и наносит {enemyDamage} урона.");

                    Thread.Sleep(1000); // Задержка в 1 секунду

                    if (player.Health <= 0)
                    {
                        Console.WriteLine("Вы проиграли. Начните заново.");
                        player = new Character(1, 30, 10);
                        break;
                    }
                }

                // Очищаем консоль и выводим информацию о победителе
                Console.Clear();
                Console.WriteLine("Бой завершен!");

                if (player.Health > 0)
                {
                    Console.WriteLine("Вы победили!");
                    Console.WriteLine($"Здоровье после боя: {player.Health}");
                }
                else
                {
                    Console.WriteLine("Вы проиграли!");
                }

                Console.Write("Нажмите Enter для продолжения или Q для выхода: ");
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    Console.WriteLine("До свидания!");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз.");
            }
        }
    }

    static List<Enemy> GenerateEnemies(int playerLevel, int count)
    {
        List<Enemy> enemies = new List<Enemy>();
        Random random = new Random();

        for (int i = 0; i < count; i++)
        {
            int enemyType = random.Next(1, 4); // Randomly choose enemy type

            switch (enemyType)
            {
                case 1:
                    enemies.Add(new NormalEnemy(playerLevel));
                    break;
                case 2:
                    enemies.Add(new MiniBossEnemy(playerLevel));
                    break;
                case 3:
                    enemies.Add(new BossEnemy(playerLevel));
                    break;
            }
        }

        return enemies;
    }

    static void DisplayEnemyList(List<Enemy> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Console.WriteLine($"{i}. Враг уровня {enemies[i].Level} (Здоровье: {enemies[i].Health}, Урон: {enemies[i].AttackDamage})");
        }
    }

    static int CalculateDamage(int baseDamage)
    {
        Random random = new Random();
        double damageMultiplier = random.NextDouble() * (1.5 - 0.5) + 0.5; // Разброс урона от 0.5 до 1.5
        return (int)(baseDamage * damageMultiplier);
    }
}
