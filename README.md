# Conway's Game of Life

This repository contains a Unity project implementing Conway's Game of Life. The Game of Life is a cellular automaton devised by mathematician John Conway, which demonstrates emergent behavior and complex patterns from simple rules.

## How to Play

The Game of Life simulation starts with an initial pattern of live cells. The simulation progresses by applying the following rules to each cell:

- Any live cell with fewer than two live neighbors dies (underpopulation).
- Any live cell with two or three live neighbors survives.
- Any live cell with more than three live neighbors dies (overpopulation).
- Any dead cell with exactly three live neighbors becomes a live cell (reproduction).

You can interact with the simulation using the following controls:

- Use the pattern selection dropdown in the GameManager component to choose different initial patterns.
- Adjust the interval time in the GameManager component to control the speed of the simulation.
- The population, iterations, and time statistics are displayed in the GameManager component.

Feel free to experiment with different patterns, adjust the simulation parameters, and observe the fascinating patterns that emerge in the Game of Life.

## Scripts

### GameManager

The `GameManager` script manages the main logic of the Game of Life simulation. It handles the initialization of the grid, updating the state of cells, and transitioning between generations. It also provides statistics about the population, iterations, and time elapsed.

### Pattern

The `Pattern` script is a ScriptableObject that stores the data for different patterns used in the Game of Life simulation. It contains an array of `Vector2Int` cells, representing the positions of live cells in each pattern. The `GetCenter()` method calculates the center point of the pattern.

## Contributing

Contributions to the project are welcome! If you have any improvements, bug fixes, or new features, feel free to open a pull request. Please provide a clear description of the changes and any related issues.
