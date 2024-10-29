import random

def display_welcome_message():
    """Display a welcome message to the user."""
    print("Welcome to the Knife Throwing Game!")

def check_allowed_to_play():
    """Ask the user if they are allowed to play the game."""
    while True:
        response = input("Are you allowed to play this game? (yes/no): ").strip().lower()
        if response in ["yes", "no"]:
            return response == "yes"
        print("Please enter 'yes' or 'no'.")

def prompt_username():
    """Prompt the user for their desired username."""
    return input("Please enter your desired username: ").strip()

def select_weapon(weapon_names, weapon_descriptions):
    """Allow the user to choose a weapon from the provided lists."""
    while True:
        print("Choose your weapon:")
        for index, (name, description) in enumerate(zip(weapon_names, weapon_descriptions), start=1):
            print(f"{index}. {name}: {description}")
        
        weapon_choice = input("Enter the number of your weapon choice: ")
        
        try:
            choice_index = int(weapon_choice) - 1
            if 0 <= choice_index < len(weapon_names):
                return weapon_names[choice_index]
            print("Invalid choice. Please choose a valid weapon number.")
        except ValueError:
            print("Invalid input. Please enter a number.")

def prompt_throw_position():
    """Get the user's throw position, ensuring it's valid."""
    while True:
        try:
            position = int(input("Enter your throw position (1-10): "))
            if 1 <= position <= 10:
                return position
            print("Position must be between 1 and 10.")
        except ValueError:
            print("Invalid input. Please enter a number.")

def perform_knife_throw():
    """Simulate a knife throw and check if it hits the target."""
    target_position = random.randint(1, 10)  # Random target position
    throw_position = prompt_throw_position()
    
    print(f"Target is at position {target_position}.")
    if abs(throw_position - target_position) <= 2:
        print("Hit!")
        return True
    else:
        print(f"Missed! You were {abs(throw_position - target_position)} positions away.")
        return False

def main():
    """Main function to run the Knife Throwing Game."""
    display_welcome_message()

    if not check_allowed_to_play():
        print("Please log out. You are not allowed to play.")
        return

    username = prompt_username()
    
    weapon_names = ["Dagger", "Sword", "Axe"]
    weapon_descriptions = [
        "A small blade, perfect for quick throws.",
        "A longer blade, provides greater distance.",
        "A heavy tool, designed for powerful throws."
    ]
    
    chosen_weapon = select_weapon(weapon_names, weapon_descriptions)
    
    print(f"{username}, you have chosen the {chosen_weapon}!")

    score = 0  # Integer to track the score
    attempts = 0  # Integer to track the number of attempts
    throw_results = []  # List to store results of each throw

    while True:
        print("\nTime to throw your knife!")
        
        # Allow multiple throws in one round
        for _ in range(3):  # Allow 3 attempts in each round
            hit = perform_knife_throw()
            attempts += 1
            
            if hit:
                score += 1
                throw_results.append("Hit")
            else:
                throw_results.append("Miss")
            
            print(f"Score: {score} | Attempts: {attempts}")

        # Ask if the user wants to continue playing or exit
        continue_playing = input("Do you want to play another round? (yes/no): ").strip().lower()
        if continue_playing != "yes":
            break  # Breaks the main loop to end the game

    # Display results
    print("\nGame Results:")
    for index, result in enumerate(throw_results, start=1):
        print(f"Throw {index}: {result}")
    
    print("Thank you for playing!")

if __name__ == "__main__":
    main()
