def main():
    # Show a welcome message
    print("Welcome to the Knife/Weapon Game!")

    # Ask if they are allowed to play
    allowed_to_play = input("Are you allowed to play this game? (yes/no): ").strip().lower()

    if allowed_to_play != "yes":
        print("Please log out. You are not allowed to play.")
        return  # End the program

    # If yes, ask for a username
    username = input("Please enter your desired username: ").strip()

    # Give the user 3 options for their first weapon choice
    weapons = ["Dagger", "Sword", "Axe"]
    print("Choose your weapon:")
    for idx, weapon in enumerate(weapons, start=1):
        print(f"{idx}. {weapon}")

    weapon_choice = input("Enter the number of your weapon choice: ")
    
    try:
        choice_index = int(weapon_choice) - 1
        if 0 <= choice_index < len(weapons):
            print(f"{username}, you have chosen the {weapons[choice_index]}!")
        else:
            print("Invalid choice. Please restart the game.")
    except ValueError:
        print("Invalid input. Please enter a number.")

    # End the program
    print("Thank you for playing!")

if __name__ == "__main__":
    main()