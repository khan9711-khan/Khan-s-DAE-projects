import tkinter as tk
import random
import math

# Game constants
WIDTH, HEIGHT = 800, 600
TARGET_RADIUS = 50

class KnifeThrowingGame:
    def __init__(self, master):
        self.master = master
        self.master.title("Knife Throwing Game")
        self.canvas = tk.Canvas(master, width=WIDTH, height=HEIGHT, bg='white')
        self.canvas.pack()
        
        self.target_x = WIDTH // 2
        self.target_y = HEIGHT // 2
        self.score = 0
        self.attempts = 0

        self.draw_target()
        self.canvas.bind("<Button-1>", self.throw_knife)

        self.score_label = tk.Label(master, text=f"Score: {self.score} Attempts: {self.attempts}")
        self.score_label.pack()

    def draw_target(self):
        """Draw the target on the canvas."""
        self.canvas.delete("target")  # Remove previous target
        self.canvas.create_oval(
            self.target_x - TARGET_RADIUS, 
            self.target_y - TARGET_RADIUS,
            self.target_x + TARGET_RADIUS, 
            self.target_y + TARGET_RADIUS,
            fill='red', 
            tags="target"
        )

    def throw_knife(self, event):
        """Handle the knife throw event."""
        mouse_x, mouse_y = event.x, event.y
        distance = math.sqrt((mouse_x - self.target_x) ** 2 + (mouse_y - self.target_y) ** 2)

        if distance <= TARGET_RADIUS:
            self.score += 1
            result = "Hit!"
        else:
            self.attempts += 1
            result = "Missed!"

        self.update_score_label()
        self.draw_target()
        print(result)

    def update_score_label(self):
        """Update the score display."""
        self.score_label.config(text=f"Score: {self.score} Attempts: {self.attempts}")

if __name__ == "__main__":
    root = tk.Tk()
    game = KnifeThrowingGame(root)
    root.mainloop()
