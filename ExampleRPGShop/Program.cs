using System;
using System.Collections.Generic;
using System.IO;

namespace ExampleRPGShop
{
    class Program
    {
        //Global Variables//
        public static List<ShopItem> shopItemsList = new List<ShopItem>();
        public static List<ShopItem> playerInventory = new List<ShopItem>();

        static int playerGold = 500;
        static float sellingPriceMultiplier = 0.8f;

        static string input;
        ////////////////////
        
        static void Main(string[] args)
        {
            InputPlayerInventory();
            InputShopItems();

            StartShop();
        }

        static void InputPlayerInventory()
        {
            StreamReader reader = new StreamReader("../../PlayerInventory.txt");

            //Reads the first line from text
            string data = reader.ReadLine();

            //Continues reading lines until it reaches the end of the file
            while (data != null)
            {
                //Splits a string into an array of strings using ',' as the delimiter
                string[] playerInventorySplitAr = data.Split(',');

                //for every string in the aplit array, it splits that string into a name and a type
                for (int i = 0; i < playerInventorySplitAr.Length; i++)
                {
                    string[] itemAr = playerInventorySplitAr[i].Split(':');
                    int type = int.Parse(itemAr[1]);
                    string name = itemAr[0];
                    int price = int.Parse(itemAr[2]);

                    playerInventory.Add(new ShopItem(name, price, type));
                }

                data = reader.ReadLine();
            }

            reader.Close();
        }

        static void OutputPlayerInventory()
        {
            StreamWriter writer = new StreamWriter("../../PlayerInventory.txt");

            //Loop through player inventory
            for (int i = 0; i < playerInventory.Count; i++)
            {
                //write comma if it isn't before the first item
                if (i != 0)
                {
                    writer.Write(",");
                }

                //writes the shop item values out
                writer.Write($"{playerInventory[i].GetName()}:{(int)playerInventory[i].GetType()}:{playerInventory[i].GetValue()}");
            }

            writer.Close();
        }

        static void InputShopItems()
        {
            StreamReader reader = new StreamReader("../../ShopItems.txt");
            Random random = new Random();

            //Reads the first line from text
            string data = reader.ReadLine();

            //Continues reading lines until it reaches the end of the file
            while(data != null)
            {
                //Splits a string into an array of strings using ',' as the delimiter
                string[] shopItemsSplitAr = data.Split(',');

                //for every string in the aplit array, it splits that string into a name and a type
                for (int i = 0; i < shopItemsSplitAr.Length; i++)
                {
                    string[] itemAr = shopItemsSplitAr[i].Split(':');
                    int type = int.Parse(itemAr[1]);
                    string name = itemAr[0];

                    shopItemsList.Add(new ShopItem(name, random.Next(25, 500), type));
                }

                data = reader.ReadLine();
            }

            reader.Close();
        }

        static void StartShop()
        {
            Console.WriteLine("Welcome to our shop! What would you like to do? \n");

            Console.WriteLine("1. Buy \n\n2. List \n\n3. Sell \n\n4. Trade \n\n5. Exit \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice == 1)
            {
                BuySection();
            }
            else if (choice == 2)
            {
                ListSection();
            }
            else if (choice == 3)
            {
                SellSection();
            }
            else if (choice == 4)
            {
                TradeSection();
            }
            else if (choice == 5)
            {
                OutputPlayerInventory();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                StartShop();
            }

        }

        static void SellSection()
        {
            if (playerInventory.Count == 0)
            {
                Console.WriteLine("You do not have any items to sell. \n");
                StartShop();
            }

            Console.WriteLine("What would you like to Sell? \n");

            for (int i = 0; i < playerInventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {playerInventory[i].GetName()} \nSelling Price: {(int)(playerInventory[i].GetValue() * sellingPriceMultiplier)} Gold \n");
            }

            Console.WriteLine($"{playerInventory.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice <= playerInventory.Count)
            {
                Console.WriteLine($"Would you like to sell {playerInventory[choice - 1].GetName()} for {(int)(playerInventory[choice - 1].GetValue() * sellingPriceMultiplier)} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("Thank you! here is your payment\n");
                    Console.WriteLine($"Player received {(int)(playerInventory[choice - 1].GetValue() * sellingPriceMultiplier)} Gold for their {playerInventory[choice - 1].GetName()} \n");

                    for (int i = 0; i < playerInventory.Count; i++)
                    {
                        if (playerInventory[i] == playerInventory[choice - 1])
                        {
                            shopItemsList.Add(playerInventory[choice - 1]);
                            playerGold += (int)(playerInventory[choice - 1].GetValue() * sellingPriceMultiplier);
                            playerInventory.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    SellSection();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    SellSection();
                }
            }
            else if (choice == playerInventory.Count + 1)
            {
                StartShop();
            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                SellSection();
            }

            StartShop();
        }

        static void ListSection()
        {
            if (playerInventory.Count == 0)
            {
                Console.WriteLine("You do not have any items to list. \n");
                StartShop();
            }

            Console.WriteLine("Here are your items! \n");

            for (int i = 0; i < playerInventory.Count; i++)
            {
                Console.WriteLine($"{playerInventory[i].GetName()} \nItem Value: {(int)(playerInventory[i].GetValue())} Gold \n");
            }

            Console.WriteLine($"{1}. Back to Shop \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice == 1)
            {
                StartShop();
            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                ListSection();
            }

            StartShop();
        }

        static void TradeSection()
        {
            List<ShopItem> tradeList = new List<ShopItem>();
            int count = 1;

            if (playerInventory.Count == 0)
            {
                Console.WriteLine("You do not have any items to trade. \n");
                StartShop();
            }

            Console.WriteLine("These are your trading options, these options are made based on similarly valued items.");

            for (int i = 0; i < playerInventory.Count; i++)
            {
                for (int j = 0; j < shopItemsList.Count; j++)
                {
                    if (Math.Abs(playerInventory[i].GetValue() - shopItemsList[j].GetValue()) <= 50)
                    {
                        tradeList.Add(playerInventory[i]);
                        tradeList.Add(shopItemsList[j]);
                    }
                }
            }

            for (int i = 0; i < tradeList.Count; i += 2)
            {
                Console.WriteLine($"{count}. {tradeList[i].GetName()} for {tradeList[i + 1].GetName()} \n");
                count++;
            }

            Console.WriteLine($"{count}. Cancel");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice < count)
            {
                Console.WriteLine($"Please confirm this trade: \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("The trade has been completed you have received your new item\n");


                    List<ShopItem> swapItems = new List<ShopItem>();

                    int itemIndex = (choice - 1) * 2;

                    swapItems.Add(tradeList[itemIndex]);
                    swapItems.Add(tradeList[itemIndex + 1]);
                    for (int i = 0; i < playerInventory.Count; i++)
                    {
                        if (playerInventory[i] == swapItems[0])
                        {
                            shopItemsList.Add(swapItems[0]);
                            playerInventory.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == swapItems[1])
                        {
                            playerInventory.Add(swapItems[1]);
                            shopItemsList.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    TradeSection();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    TradeSection();
                }
            }
            else if (choice == count)
            {
                StartShop();
            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                TradeSection();
            }

            StartShop();
        }

        #region BUY FUNCTIONS
        static void BuySection()
        {
            Console.WriteLine($"You have {playerGold} gold \nWhat would you like to buy? \n");
            Console.WriteLine("1. Weapons \n\n2. Armour \n\n3. Consumables \n\n4. Cancel\n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice == 1)
            {
                BuyWeapon();
            }
            else if (choice == 2)
            {
                BuyArmour();
            }
            else if (choice == 3)
            {
                BuyConsumables();
            }
            else if (choice == 4)
            {
                StartShop();
            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuySection();
            }

            StartShop();
        }

        static void BuyWeapon()
        {
            List<ShopItem> weaponsList = new List<ShopItem>();

            for(int i = 0; i < shopItemsList.Count; i++)
            {
                if (shopItemsList[i].GetType() == ShopItem.ItemType.Weapon)
                    weaponsList.Add(shopItemsList[i]);
            }

            Console.WriteLine($"Gold: {playerGold} \nWhich weapon would you like to buy: \n");

            for (int i = 0; i < weaponsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {weaponsList[i].GetName()} \nPrice: {weaponsList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{weaponsList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice <= weaponsList.Count)
            {
                if (playerGold < weaponsList[choice - 1].GetValue())
                {
                    Console.WriteLine("You do not have enough gold, pick a different option\n");
                    BuyWeapon();
                }

                Console.WriteLine($"Would you like to buy {weaponsList[choice - 1].GetName()} for {weaponsList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("You have recieved your new weapon\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == weaponsList[choice - 1])
                        {
                            playerInventory.Add(weaponsList[choice - 1]);
                            playerGold -= weaponsList[choice - 1].GetValue();
                            shopItemsList.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    BuyWeapon();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    BuyWeapon();
                }
            }
            else if (choice == weaponsList.Count + 1)
            {
                BuySection();
            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuyWeapon();
            }

        }

        static void BuyArmour()
        {
            List<ShopItem> armourList = new List<ShopItem>();

            for (int i = 0; i < shopItemsList.Count; i++)
            {
                if (shopItemsList[i].GetType() == ShopItem.ItemType.Armour)
                    armourList.Add(shopItemsList[i]);
            }

            Console.WriteLine($"Gold: {playerGold} \nWhich item would you like to buy: \n");

            for (int i = 0; i < armourList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {armourList[i].GetName()} \nPrice: {armourList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{armourList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice <= armourList.Count)
            {
                if (playerGold < armourList[choice - 1].GetValue())
                {
                    Console.WriteLine("You do not have enough gold, pick a different option\n");
                    BuyArmour();
                }

                Console.WriteLine($"Would you like to buy {armourList[choice - 1].GetName()} for {armourList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("You have received your armour\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == armourList[choice - 1])
                        {
                            playerInventory.Add(armourList[choice - 1]);
                            playerGold -= armourList[choice - 1].GetValue();
                            shopItemsList.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    BuyArmour();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    BuyArmour();
                }
            }
            else if (choice == armourList.Count + 1)
            {
                BuySection();
            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuyArmour();
            }
        }

        static void BuyConsumables()
        {
            List<ShopItem> consumablesList = new List<ShopItem>();

            for (int i = 0; i < shopItemsList.Count; i++)
            {
                if (shopItemsList[i].GetType() == ShopItem.ItemType.Consumable)
                    consumablesList.Add(shopItemsList[i]);
            }

            Console.WriteLine($"Gold: {playerGold} \nWhich item would you like to buy: \n");

            for (int i = 0; i < consumablesList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {consumablesList[i].GetName()} \nPrice: {consumablesList[i].GetValue()} Gold \n");
            }

            Console.WriteLine($"{consumablesList.Count + 1}. Cancel \n");

            input = Console.ReadLine();

            int.TryParse(input, out int choice);

            Console.Clear();

            if (choice <= consumablesList.Count)
            {
                if (playerGold < consumablesList[choice - 1].GetValue())
                {
                    Console.WriteLine("You do not have enough gold, pick a different option\n");
                    BuyConsumables();
                }

                Console.WriteLine($"Would you like to buy {consumablesList[choice - 1].GetName()} for {consumablesList[choice - 1].GetValue()} Gold? \n" +
                                   "1. Yes \n\n2. No\n");

                input = Console.ReadLine();

                int.TryParse(input, out int confirm);

                Console.Clear();

                if (confirm == 1)
                {
                    Console.WriteLine("You have received your new item\n");

                    for (int i = 0; i < shopItemsList.Count; i++)
                    {
                        if (shopItemsList[i] == consumablesList[choice - 1])
                        {
                            playerInventory.Add(consumablesList[choice - 1]);
                            playerGold -= consumablesList[choice - 1].GetValue();
                            shopItemsList.RemoveAt(i);
                        }
                    }
                }
                else if (confirm == 2)
                {
                    BuyConsumables();
                }
                else
                {
                    Console.WriteLine("That is not a valid option.\n");
                    BuyConsumables();
                }
            }
            else if (choice == consumablesList.Count + 1)
            {
                BuySection();
            }
            else
            {
                Console.WriteLine("That is not a valid option, Please choose a valid option. \n");
                BuyConsumables();
            }
        }
        #endregion
    }
}
