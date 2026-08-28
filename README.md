# battleships

> [!NOTE]
>
> This project is a small experiment where I asked an AI to whip up a simple board game in Visual Studio. The idea was to see how much instruction and prompting it actually needed to take the app from the very first request all the way to a running, working build.

**Asking an AI Agent to build a simple app from scratch**

I begin by asking the AI to review my draft request and suggest improvements so the Agent has a clearer understanding of the requirements.

Below is the conversation. The results were mixed.

#### Table of Contents
* [My opening shot - help me help you help me](#my-opening-question---help-me-help-you-help-me)
* [AI's response, including first draft](#ais-response-including-first-draft)
* [My follow up question about formatting](#my-follow-up-question-about-formatting)
* [AI's summarised answer, including improvements to its initial response](#ais-summarised-answer-including-improvements-to-its-initial-response)
* [My next question about asking AI to follow best practice](#my-next-question-about-asking-ai-to-follow-best-practice)
* [AI's response, including further refinement](#ais-response-including-further-refinement)
* [Me asking AI to for the final draft](#me-asking-ai-to-for-the-final-draft)
* [AI providing the final consolidated version](#ai-providing-the-final-consolidated-version)
* [The results were mixed](#the-results-were-mixed)
  * [Prompt 1. Add missing projects to solution and fix all project references](#prompt-1-add-missing-projects-to-solution-and-fix-all-project-references)
  * [Prompt 2. Ensure the unit tests are discoverable and runnable](#prompt-2-ensure-the-unit-tests-are-discoverable-and-runnable)
  * [Prompt 3. Add startup routine and user‑input handling logic to `Program.cs`](#prompt-3-add-startup-routine-and-userinput-handling-logic-to-programcs)
  * [Finally up and running](#finally-up-and-running)

# My opening shot - help me help you help me
```
I want to ask the agent in my Visual Studio IDE to build a C# app from an empty solution. I simply want to know what is the best way to structure my request.
 
Here is my first attempt. Can it be improved for the agent to understand better? 
 
Build a single player board game application. 
 
The solution must contain: 
- A console app called "Battleships.Console", which loads an input file at the same location of the exe, and then takes instructions from the user. 
- A class library called "Battleships", for the game logic. 
- A MSTest unit test project, with test cases testing valid and invalid scenarios for the loading various input files and user input.  
 
The rules of the game is as follows: 
- The board consists of 10 rows from 1 to 10, and 10 columns from A to J. 
- There are several types of ships. Each has a Class and Size. The size determines the number of cells it takes on the board e.g. a size 5 carrier ship occupies 5 spaces on the board 
- At startup the game must load a csv file called "Battleships.csv", containing a list of ships and each ships position on the board according to its size. 
- Ships cannot sit diagonally on the board. 
- Ships cannot overlap on the board. 
- The user enters a coordinate on the board, if part of the ship is positioned at the coordinate a hit is recorded against that ship. 
- If all the coordinates for the size of the ship has been  
 
Here are the ships according to class and size: 
- Carrier,5 
- Battleship,4 
- Cruiser,3 
- Submarine,3 
- Destroyer,2 
 
User console input: 
- The user must be able to enter a valid alphanumeric coordinate in the format [RowColumn] e.g. the coordinate for column B and row 3 is B3 
- If the user presses the Escape key the game ends. 
 
Output written to the console: 
- At startup, if a ship takes more or fewer cells than its size, the console must output "Incorrect ship size", and the game cannot start. 
- At startup, if two or more ships overlap on the board the console must output "Ships cannot overlap", and the game cannot start. 
- At startup, if one of more ships sits diagonally on the board the console mut output "Ships cannot sit diagonally on the board", and the game cannot start. 
- At startup, if the ship isn't a valid ship the console must output "Ships not recognized", and the game cannot start. 
- At startup, if the ships are correctly positioned on the board the console must output "Battleships Begin", and the game can start.  
- If a user enters an invalid coordinate the console must output "Invalid coordinate". 
- The console must write "Hit", "Miss" or "Sunk", depending on whether a ship is positioned at that coordinate. 
- If a user enters the same coordinate twice the console must output "Cannot enter same coordinate twice". 
- If all the coordinates covering the position of a ship have been hit, the console must write "Sunk". 
- When all the ships are sunk, the console must write "Game Over".

Example "Battleships.csv" files:

1. File contains valid contents
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7 
Cruiser,G5,H5,i5 
Submarine,E9,F9,G9 
Destroyer,I7,I8 

2. File contains invalid coordinates
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7 
Cruiser,G5,H5,i5 
Submarine,E9,F9,G9 
Destroyer,K7,K8 

3. File contains invalid ship
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7 
Cruiser,G5,H5,i5 
Submarine,E9,F9,G9 
Destroyer,I7,I8 
Boat,A1

4. File contains invalid ship size
Carrier,C2,D2,E2
Battleship,D4,D5,D6,D7 
Cruiser,G5,H5,i5 
Submarine,E9,F9,G9 
Destroyer,I7,I8 

5. File contains diagonal ship coordinates
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7 
Cruiser,G5,H5,i5 
Submarine,E9,F9,G9 
Destroyer,H7,I8 

6. File contains ship overlap
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7 
Cruiser,G5,H5,i5 
Submarine,E9,F9,G9 
Destroyer,G9,H9 
```

# AI's response, including first draft

Yes. Your first attempt is already quite understandable, but an IDE coding agent will perform better if you make the requirements more deterministic: define the solution structure, input format, validation rules, expected behaviour, and acceptance criteria separately.

The biggest improvements I’d make are: remove ambiguity about coordinate ordering and case sensitivity; explicitly say ships must occupy contiguous cells; specify what happens when several startup validation errors exist; tell the agent not to over-engineer; and give it a clear definition of “done.”

I’d structure the request something like this:

```
Build a complete C# Battleships application from the current empty Visual Studio solution.

## Objective

Create a simple single-player Battleships console application.

Keep the design clean and object-oriented, but do not over-engineer it. Game logic must be separated from console/input-output concerns so that the logic can be unit tested independently.

Use the .NET version currently targeted by the solution/environment.

## Solution Structure

Create these projects:

1. Battleships
   - C# class library.
   - Contains all game/domain logic.
   - Must not depend on Console APIs.

2. Battleships.Console
   - C# console application.
   - References Battleships.
   - Loads Battleships.csv.
   - Handles user input and console output.

3. Battleships.Tests
   - MSTest project.
   - References Battleships.
   - Contains unit tests covering valid and invalid scenarios.

Configure the project references as necessary and ensure the complete solution builds successfully.

## Board

The board is 10 x 10.

Columns:
A, B, C, D, E, F, G, H, I, J

Rows:
1 through 10

Coordinates are written as Column followed by Row.

Examples:
A1
B3
J10

Coordinate letters must be treated case-insensitively, so "i5" and "I5" are equivalent.

Coordinates outside A-J or 1-10 are invalid.

## Ships

The only valid ship classes and sizes are:

Carrier     = 5
Battleship  = 4
Cruiser     = 3
Submarine   = 3
Destroyer   = 2

Each ship must appear exactly once in the input file.

A ship must:
- occupy exactly the number of cells specified by its size;
- be positioned horizontally or vertically;
- occupy contiguous cells without gaps;
- remain entirely within the board;
- not overlap another ship.

Diagonal ships are invalid.

## Input File

At application startup, Battleships.Console must load:

Battleships.csv

The file must be loaded from the same directory as the executable.

Each non-empty line represents one ship.

Format:

ShipClass,Coordinate1,Coordinate2,...

Example:

Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,I7,I8

Trim surrounding whitespace from individual CSV values.

## Startup Validation

Validate the complete input file before starting the game.

The application must detect at least:

1. Unknown ship class
   Output:
   Ships not recognised

2. Incorrect number of coordinates for a ship
   Output:
   Incorrect ship size

3. Invalid or out-of-board coordinate
   Output:
   Invalid coordinate

4. Ship coordinates that are not all on one horizontal or vertical line
   Output:
   Ships cannot sit diagonally on the board

5. Ship coordinates that are not contiguous
   Treat this as invalid ship positioning and use:
   Ships cannot sit diagonally on the board

6. Two ships occupying the same coordinate
   Output:
   Ships cannot overlap

If startup validation fails:
- output the appropriate error;
- do not start the game;
- terminate the application.

If more than one error exists, reporting the first error found is sufficient.

If the file is valid, output:

Battleships Begin

and begin accepting player input.

## Player Input

While the game is running, accept board coordinates such as:

A1
B3
J10

Input must be case-insensitive.

If a coordinate is malformed or outside the board, output:

Invalid coordinate

and allow the player to try again.

If the player enters a coordinate that has already been entered, output:

Cannot enter same coordinate twice

and allow the player to try again.

Pressing the Escape key must immediately end the game.

Do not treat Escape as a coordinate.

## Shot Results

For each new valid coordinate:

- If no ship occupies the coordinate, output:
  Miss

- If a ship occupies the coordinate but still has un-hit cells remaining, output:
  Hit

- If the shot hits the final un-hit cell of that ship, output:
  Sunk

A coordinate can only be hit once.

When all five ships have been sunk, output:

Game Over

and end the game.

## Examples of Input Files

### Valid

Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,I7,I8

### Invalid coordinate

Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,K7,K8

Expected startup error:

Invalid coordinate

### Invalid ship

Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,I7,I8
Boat,A1

Expected startup error:

Ships not recognised

### Incorrect ship size

Carrier,C2,D2,E2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,I7,I8

Expected startup error:

Incorrect ship size

### Diagonal ship

Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,H7,I8

Expected startup error:

Ships cannot sit diagonally on the board

### Overlapping ships

Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,G9,H9

Expected startup error:

Ships cannot overlap

## Unit Tests

Create comprehensive MSTest tests for the game logic.

At minimum, test:

- valid board configuration;
- each valid ship type and size;
- horizontal ships;
- vertical ships;
- lowercase coordinate input;
- malformed coordinates;
- coordinates outside A-J;
- rows outside 1-10;
- unknown ship classes;
- incorrect ship sizes;
- diagonal ships;
- non-contiguous ships;
- overlapping ships;
- successful hit;
- miss;
- sinking a ship;
- attempting the same coordinate twice;
- completion of the game after every ship is sunk.

Where practical, test the class library rather than testing Console.ReadLine/Console.WriteLine directly.

## Implementation Expectations

Use sensible domain types rather than putting all logic in Program.cs.

For example, consider concepts such as:
- Coordinate
- Ship
- ShipType
- Board or Game
- input-file parser/loader

These names are suggestions, not mandatory.

Prefer small classes with clear responsibilities.

Do not introduce unnecessary frameworks, databases, dependency-injection containers, UI frameworks, or third-party packages.

## Definition of Done

Before finishing:

1. Build the complete solution.
2. Fix all compiler errors.
3. Run all MSTest tests.
4. Fix any failing tests.
5. Ensure the console project can run with the valid Battleships.csv example.
6. Ensure game logic is contained in Battleships rather than Battleships.Console.
7. Give me a brief summary of:
   - the files/classes created;
   - the design used;
   - the tests created;
   - any assumptions you had to make.

Do not stop after scaffolding the projects. Implement the complete working application and tests.
```

There are a few important differences between this and your original version.

First, “contiguous” needs to be explicit. Your original rules prevent diagonal ships but technically allow a Carrier at `A1,C1,E1,G1,I1`, since all five coordinates are horizontal and don't overlap. Presumably that should be illegal.

Second, your original prompt says:

> “If all the coordinates for the size of the ship has been…”

That sentence is unfinished. I assume you mean that when every cell belonging to a ship has been hit, the ship is sunk. Making that explicit removes an important ambiguity.

Third, you gave the agent an invalid-coordinate CSV example, but didn't specify the startup console message for it. I used `"Invalid coordinate"` because it matches your user-input error. If you want a different startup error, define it explicitly.

Fourth, I would explicitly state that coordinates are case-insensitive, because your own valid example contains `i5` rather than `I5`. Otherwise an agent could reasonably implement lowercase coordinates as invalid.

I'd also strongly recommend the final “Definition of Done” section. Coding agents sometimes interpret “build an app” as “create the files and implement most of it.” Telling it to actually compile, run the test suite, repair failures, and only then finish tends to produce much better results.

One further improvement, if you're using an agent capable of making changes autonomously, is to give it permission to make reasonable implementation decisions without repeatedly asking you questions. For example, add:

```
If an implementation detail is not specified above, make the simplest
reasonable choice consistent with these requirements and document the
assumption in your final summary rather than stopping to ask me.
```

That tends to be particularly useful for this kind of exercise.

I would not prescribe the exact classes, interfaces, methods, or design patterns unless this is an exercise where you're specifically assessing architecture. Saying “consider `Coordinate`, `Ship`, `Board`, etc.” gives the agent enough direction while still allowing it to design the solution. If you tell it exactly what every class must look like, you're effectively doing part of the programming task in the prompt rather than stating the requirements.

# My follow up question about formatting

```
Is your recommended request structure in format I can copy and paste into the Agent chat window in my IDE?

Also, what's with the use of # symbols in the request? How do they add value to the request?
```

# AI's summarised answer, including improvements to its initial response

Yes. The version I gave you is intended to be copied and pasted directly into the Agent chat window. You don't need to convert it to another format.

The # symbols are Markdown headings. They're not special instructions to the agent and aren't C# syntax in this context.

The agent would probably understand the request without them, but headings give the prompt a clear hierarchy. They separate things such as:
- Objective
- Solution Structure
- Board
- Ships
- Input File
- Startup Validation
- Player Input
- Unit Tests
- Definition of Done

This matters particularly for a long software specification. It helps the model distinguish requirements from examples, rules from test cases, and implementation guidance from acceptance criteria.

**One change I'd make for your actual Agent prompt**

Since you're starting from an empty solution, I'd put a short instruction right at the top that establishes the agent's role and tells it to work autonomously:

```
You are working in an empty Visual Studio solution.

Build the complete C# application described below. You may create projects, files, classes, and project references as required.

Work through the task autonomously. Do not stop after creating the project structure or producing sample code. Implement the complete application, build the solution, run the tests, and fix any compilation errors or failing tests before finishing.

If an implementation detail is not specified, make the simplest reasonable choice consistent with the requirements and mention the assumption in your final summary.

# Battleships Application

## Objective

Create a single-player Battleships console application.

...
```

That opening is useful because it tells the agent what environment it's starting from, what authority it has, how far it should take the task, and what to do when it encounters an unspecified detail.

So yes: I'd treat the longer prompt from my previous answer as essentially an agent-ready specification. The Markdown isn't required, but I recommend keeping it because it makes a specification of this size considerably easier for both you and the agent to navigate.

# My next question about asking AI to follow best practice
```
Is it necessary to specify the Agent should follow best practice such
as S.O.L.I.D and to consider algorithmic efficiency such as Big‑O?
```

# AI's response, including further refinement

Not strictly necessary, but I would add a short engineering-quality section. The important part is not to over-specify it.

For a small Battleships exercise, saying simply “follow SOLID and optimise Big-O” can actually encourage the agent to over-engineer the solution—lots of interfaces, abstractions, factories, repositories, etc. that aren't justified by the problem.

A better instruction is:

```
## Engineering Quality

Follow standard C# and .NET best practices.

* Apply SOLID principles where they provide a clear benefit, but do not introduce unnecessary abstractions or over-engineer the solution.
* Keep classes and methods focused with clear responsibilities.
* Prefer simple, readable, maintainable code over unnecessary design patterns.
* Use appropriate data structures and algorithms, considering time and space complexity where relevant.
* Avoid inefficient repeated searches or unnecessary processing when a simpler efficient approach is available.
* Use idiomatic modern C# and appropriate .NET APIs.
* Handle expected invalid input cleanly without using exceptions for normal control flow.
* Keep game/domain logic independent of console input/output so that it can be tested easily.

```

Why I'd phrase it this way

**SOLID:** Definitely relevant, particularly the **Single Responsibility Principle** and dependency separation. For example, you don't want `Program.cs` simultaneously parsing CSV, validating ships, maintaining board state, detecting hits and writing console output.

But you probably don't need an `ICoordinateFactory`, `IShipRepository`, `IBoardService`, `IGameEngineFactory`, etc. just to claim compliance with SOLID.

**Big-O:** It's worth mentioning, but I wouldn't impose a specific complexity target. Your board contains only 100 cells and five ships, so virtually any sensible implementation will be fast enough. The value is encouraging good choices.

For example, when processing a shot, an agent might search every ship and every coordinate:

`O(number of ships × ship size)`

That's perfectly acceptable at this scale. Alternatively, it might represent occupied coordinates with a `Dictionary<Coordinate, Ship>`, making ship lookup approximately `O(1)` on average. That's elegant if it naturally fits the design, but there's no reason to build a sophisticated optimisation framework around a 10×10 board.

One other instruction I'd add

Since this looks like an application where code quality is part of what you care about, I'd also tell the agent:

> Do not optimise solely to make the unit tests pass. Implement the requirements through appropriate production code and use the tests to verify that behaviour.

That helps avoid a common agent failure mode: producing code overly tailored to the exact examples and tests rather than implementing the underlying rules.

So I'd include engineering expectations, but make the priority explicit:

**correctness → simplicity → readability/maintainability → appropriate efficiency → abstraction** only where justified.

That gives the agent much better guidance than simply saying “use SOLID and Big-O.”

# Me asking AI to for the final draft
```
Provide a final request with your recommendations,
including the short instruction right at the top,
the section on engineering quality, and instruction on code quality.

Also give it permission to make reasonable implementation
decisions without repeatedly asking you questions.
```

# AI providing the final consolidated version

Below is the consolidated version I'd paste directly into the Visual Studio Agent chat. I've incorporated the original requirements, clarified the ambiguities we identified, added autonomy, engineering/code-quality guidance, and made the completion criteria explicit.

```
You are working in an empty Visual Studio solution.

Build the complete C# application described below. You may create projects, files, classes, project references, and other solution files as required.

Work through the task autonomously. Do not stop after creating the project structure or producing sample code. Implement the complete application, build the solution, run the tests, and fix any compilation errors or failing tests before finishing.

If an implementation detail is not specified, make the simplest reasonable choice consistent with the requirements. Do not repeatedly stop to ask questions about minor implementation decisions. Make reasonable engineering decisions yourself and mention any significant assumptions in your final summary.

# Battleships Application

## Objective

Create a simple single-player Battleships console application.

The application loads the positions of ships from a CSV file and allows the player to enter coordinates to fire at the ships until all ships have been sunk or the player exits the game.

Keep the design clean and object-oriented, but do not over-engineer it.

Game/domain logic must be separated from console input/output concerns so that the game logic can be unit tested independently.

Use the .NET version currently targeted by, or most appropriate for, the solution/environment.

## Solution Structure

Create these projects:

1. `Battleships`

   * C# class library.
   * Contains the game/domain logic.
   * Must not depend on console APIs.

2. `Battleships.Console`

   * C# console application.
   * References `Battleships`.
   * Loads the input file.
   * Handles user input.
   * Handles console output.

3. `Battleships.Tests`

   * MSTest unit test project.
   * References `Battleships`.
   * Contains comprehensive tests for valid and invalid scenarios.

Configure all necessary project references and ensure the complete solution builds successfully.

## Board

The board is 10 × 10.

Columns are:

A, B, C, D, E, F, G, H, I, J

Rows are:

1 through 10

Coordinates are written as Column followed by Row.

Examples:

A1
B3
J10

Coordinate letters must be treated case-insensitively. For example, `i5` and `I5` represent the same coordinate.

Coordinates outside columns A-J or rows 1-10 are invalid.

## Ships

The only valid ship classes and their required sizes are:

* Carrier = 5
* Battleship = 4
* Cruiser = 3
* Submarine = 3
* Destroyer = 2

Each of these ships must appear exactly once in a valid input file.

A ship must:

* Occupy exactly the number of cells specified by its size.
* Be positioned horizontally or vertically.
* Occupy contiguous cells without gaps.
* Remain entirely within the 10 × 10 board.
* Not overlap another ship.

Ships cannot be positioned diagonally.

The order of coordinates within a ship's CSV entry should not affect whether an otherwise valid ship is accepted.

## Input File

At application startup, `Battleships.Console` must load a CSV file named:

`Battleships.csv`

The file must be loaded from the same directory as the executable.

Each non-empty line represents one ship.

The format is:

`ShipClass,Coordinate1,Coordinate2,...`

For example:

`Carrier,C2,D2,E2,F2,G2`

Trim surrounding whitespace from individual CSV values.

Treat ship class names and coordinate column letters case-insensitively.

## Startup Validation

Validate the complete input file before starting the game.

The application must detect at least the following invalid conditions.

### Unknown Ship

If the file contains a ship class that is not recognised, output:

`Ships not recognised`

The game must not start.

### Missing or Duplicate Ship

Each of the five required ship classes must occur exactly once.

If a required ship is missing or a ship class occurs more than once, treat the fleet as invalid and output:

`Ships not recognised`

The game must not start.

### Incorrect Ship Size

If a ship contains more or fewer coordinates than its required size, output:

`Incorrect ship size`

The game must not start.

### Invalid Coordinate

If a ship contains a malformed coordinate or a coordinate outside the board, output:

`Invalid coordinate`

The game must not start.

### Invalid Ship Position

If a ship's coordinates are not all on one horizontal or vertical line, output:

`Ships cannot sit diagonally on the board`

The game must not start.

If a ship is horizontal or vertical but its cells are not contiguous, treat this as invalid ship positioning and output the same message:

`Ships cannot sit diagonally on the board`

The game must not start.

### Overlapping Ships

If two or more ships occupy the same coordinate, output:

`Ships cannot overlap`

The game must not start.

### Multiple Errors

If the file contains more than one validation error, reporting the first error encountered is sufficient.

Do not start the game after any startup validation error.

### Valid Configuration

If the file is valid, output:

`Battleships Begin`

The game can then start accepting player input.

## Player Input

While the game is running, accept board coordinates such as:

A1
B3
J10

Coordinate input must be case-insensitive.

If a coordinate is malformed or outside the board, output:

`Invalid coordinate`

The player must then be allowed to try again.

If the player enters a coordinate that has already been entered, output:

`Cannot enter same coordinate twice`

The player must then be allowed to try again.

A repeated coordinate must not cause any additional hit to be recorded against a ship.

If the user presses the Escape key, immediately end the game.

Escape must not be treated as coordinate input.

## Shot Results

For each new valid coordinate entered by the player:

* If no ship occupies the coordinate, output:
  `Miss`

* If a ship occupies the coordinate and that ship still has one or more un-hit cells remaining after the shot, output:
  `Hit`

* If the shot hits the final un-hit cell belonging to that ship, output:
  `Sunk`

Each coordinate can only be successfully fired upon once.

When every coordinate belonging to every ship has been hit, output:

`Game Over`

The application must then end the game.

If the final shot both sinks the final remaining ship and completes the game, output:

`Sunk`

followed by:

`Game Over`

## Example Input Files

### 1. Valid File
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,I7,I8

Expected startup output:

`Battleships Begin`

### 2. Invalid Coordinates
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,K7,K8

Expected startup output:

`Invalid coordinate`

### 3. Invalid Ship
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,I7,I8
Boat,A1

Expected startup output:

`Ships not recognised`

### 4. Incorrect Ship Size
Carrier,C2,D2,E2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,I7,I8

Expected startup output:

`Incorrect ship size`

### 5. Diagonal Ship
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,H7,I8

Expected startup output:

`Ships cannot sit diagonally on the board`

### 6. Overlapping Ships
Carrier,C2,D2,E2,F2,G2
Battleship,D4,D5,D6,D7
Cruiser,G5,H5,I5
Submarine,E9,F9,G9
Destroyer,G9,H9

Expected startup output:

`Ships cannot overlap`

## Unit Tests

Create comprehensive MSTest tests for the game/domain logic.

At minimum, test:

* A valid complete board configuration.
* Each valid ship class and its required size.
* Horizontal ship positioning.
* Vertical ship positioning.
* Coordinates supplied in a different order.
* Lowercase coordinate input.
* Lowercase ship class names.
* Malformed coordinates.
* Columns outside A-J.
* Rows below 1.
* Rows above 10.
* Unknown ship classes.
* Missing required ships.
* Duplicate ship classes.
* Ships containing too few coordinates.
* Ships containing too many coordinates.
* Diagonal ships.
* Non-contiguous horizontal ships.
* Non-contiguous vertical ships.
* Overlapping ships.
* A successful hit.
* A miss.
* Multiple hits against the same ship.
* Sinking a ship.
* Attempting the same coordinate twice.
* Ensuring a repeated coordinate does not count as another hit.
* Completion of the game after every ship has been sunk.

Include additional meaningful edge-case tests where appropriate.

Where practical, test the `Battleships` class library directly rather than testing `Console.ReadLine`, `Console.ReadKey`, or `Console.WriteLine`.

Do not create tests merely to reproduce the implementation. Tests should verify the externally observable behaviour and business rules.

## Engineering Quality

Follow standard modern C# and .NET engineering practices.

Apply SOLID principles where they provide a clear benefit, but do not introduce unnecessary abstractions solely for the purpose of demonstrating SOLID.

In particular:

* Keep classes and methods focused with clear responsibilities.
* Maintain a clear separation between domain/game logic and console input/output.
* Prefer simple, readable and maintainable code.
* Avoid unnecessary design patterns, layers, interfaces, factories, or abstractions.
* Use appropriate encapsulation.
* Avoid unnecessary public members.
* Use meaningful names for types, methods, properties and variables.
* Avoid duplicated business logic.
* Use idiomatic modern C# and appropriate .NET APIs.
* Handle expected invalid input cleanly rather than using exceptions as normal control flow.
* Use exceptions where genuinely exceptional conditions warrant them.
* Avoid unnecessary mutable state.
* Do not introduce third-party packages unless they are genuinely required.

Use appropriate data structures and algorithms.

Consider time and space complexity where relevant, but do not introduce unnecessary optimisation for a 10 × 10 board.

Prefer an efficient and straightforward implementation over repeatedly scanning or transforming data unnecessarily when a simple lookup structure would make the design clearer.

Do not sacrifice readability or maintainability for insignificant performance improvements.

## Code Quality

Implement the actual business rules described in this specification rather than hard-coding behaviour specifically for the supplied examples or unit tests.

Do not optimise solely to make the unit tests pass.

The production code must implement the general Battleships rules, and the tests must verify that implementation.

Avoid special-case logic based on the exact coordinates or ordering used in the example CSV files.

Keep `Program.cs` focused on application startup and console interaction. Do not place the complete game implementation in `Program.cs`.

Use appropriate domain types rather than representing every concept as raw strings.

For example, consider domain concepts such as:

* Coordinate
* Ship
* ShipType
* Board
* Game
* Shot result
* Fleet/input-file parser or loader

These names and types are suggestions rather than mandatory architecture.

Choose the simplest design that cleanly models the requirements.

## Scope

Do not introduce functionality that has not been requested.

In particular, do not add:

* A graphical user interface.
* A web application.
* A database.
* Networking.
* Multiplayer functionality.
* Dependency-injection frameworks.
* Logging frameworks.
* Configuration frameworks.
* Third-party CSV libraries when the simple input format can reasonably be parsed using the .NET libraries.

Do not add complexity simply to demonstrate architectural patterns.

## Implementation Decisions and Assumptions

You have permission to make reasonable implementation decisions when the specification does not define a detail.

Prefer the simplest solution consistent with the requirements and good engineering practice.

Do not repeatedly stop and ask for clarification about minor implementation details that can reasonably be inferred.

If a genuinely significant ambiguity prevents correct implementation, ask for clarification. Otherwise, make a reasonable decision, continue working, and document the assumption in your final summary.

## Definition of Done

Do not consider the task complete until all of the following have been performed:

1. Create all three projects.
2. Configure the required project references.
3. Implement the complete application.
4. Implement the MSTest test suite.
5. Build the complete solution.
6. Fix all compiler errors.
7. Run all tests.
8. Fix any failing tests.
9. Re-run the tests and confirm they all pass.
10. Ensure the console application can run using the valid `Battleships.csv` example.
11. Ensure the game/domain logic resides in `Battleships` rather than `Battleships.Console`.
12. Review the implementation for unnecessary complexity or duplication.
13. Confirm that the implementation satisfies the requirements rather than only the supplied examples.

Do not stop after scaffolding the solution or after writing code that has not been built and tested.

## Final Response

When implementation is complete, provide a concise summary containing:

* The projects, important files and classes created.
* The overall design and separation of responsibilities.
* Any significant implementation decisions.
* Any assumptions made where the specification was not explicit.
* The unit-test coverage implemented.
* The final build result.
* The final test result, including the number of tests passed and failed.
* Any remaining limitations or issues.

If there are no known remaining issues, explicitly state that.

```

# The results were mixed

After the agent executed the request in Visual Studio, the solution was incomplete.

It created the class library project `Battleships.csproj` and the test project `Battleships.Tests.csproj`, which appeared reasonable.

The `Battleships.slnx` solution file, however, only included `Battleships.Console.csproj`. 

Most surprising the `Program.cs` was empty which meant AI failed to create any of the startup routine necessary for the game to begin.

I had to prompt the agent further to finish the job.

## Prompt 1. Add missing projects to solution and fix all project references
```
The solution `Battleships.slnx` doesnt contain `Battleships.csproj` or `Battleships.Tests.csproj`. It only contains `Battleships.Console.csproj`, which doesn't reference `Battleships.csproj`. Fix this.
```

Status after executing prompt 1
- Console project now references the library; tests reference the library.
- Build and tests previously run successfully after these project-level changes.
- Solution file still needs to include the `Battleships` and `Battleships.Tests` projects...

The agent conceded:
> I could not safely update the existing `src/Battleships.slnx` file in-place (file write/update failed in the environment)...

Fair enough. So I added the missing `Battleships` and `Battleships.Tests` projects to the solution manually to move things along.

## Prompt 2. Ensure the unit tests are discoverable and runnable
```
I have added `Battleships.Tests.csproj` to the solution but the unit tests are not discoverable. Fix this.
```

The agent failed to identify and fix the problem. There was a fair bit of back and forth between me and the agent. Despite trying to get it to understand what was wrong, it seemed to be running in circles here. 

In the end I had to manually upgrade the `nuget` test packages for the tests to discoverable and runnable. 

## Prompt 3. Add startup routine and user‑input handling logic to `Program.cs`

> [!NOTE]
>
> At this point the solution is compiling, and the tests are discoverable and runnable. Now all that is left is to get the agent to finish the job in `Program.cs` without breaking anything else! Given its ability to get things wrong I want to be explicit with my expectations.

```
Program.cs is empty but it is responsible for:
- Loads the input file.
- Handles user input.
- Handles console output.

Only make the necessary changes to Program.cs. Do not change any other file.

Here is a reminder of the requirements for the Battleships Application, for which most of the logic has already bee created in the solution.

The application loads the positions of ships from a CSV file and allows the player to enter coordinates to fire at the ships until all ships have been sunk or the player exits the game.

Board

The board is 10 × 10.

Columns are:

A, B, C, D, E, F, G, H, I, J

Rows are:

1 through 10

Coordinates are written as Column followed by Row.

Examples:

A1
B3
J10

Coordinate letters must be treated case-insensitively. For example, i5 and I5 represent the same coordinate.

Coordinates outside columns A-J or rows 1-10 are invalid.

Ships

The only valid ship classes and their required sizes are:

Carrier = 5
Battleship = 4
Cruiser = 3
Submarine = 3
Destroyer = 2

Each of these ships must appear exactly once in a valid input file.

A ship must:

Occupy exactly the number of cells specified by its size.
Be positioned horizontally or vertically.
Occupy contiguous cells without gaps.
Remain entirely within the 10 × 10 board.
Not overlap another ship.

Ships cannot be positioned diagonally.

The order of coordinates within a ship's CSV entry should not affect whether an otherwise valid ship is accepted.

Input File

At application startup, Battleships.Console must load a CSV file named:

Battleships.csv

The file must be loaded from the same directory as the executable.

Each non-empty line represents one ship.

The format is:

ShipClass,Coordinate1,Coordinate2,...

For example:

Carrier,C2,D2,E2,F2,G2

Trim surrounding whitespace from individual CSV values.

Treat ship class names and coordinate column letters case-insensitively.

Startup Validation

Validate the complete input file before starting the game.

The application must detect at least the following invalid conditions.

Unknown Ship

If the file contains a ship class that is not recognised, output:

Ships not recognised

The game must not start.

Missing or Duplicate Ship

Each of the five required ship classes must occur exactly once.

If a required ship is missing or a ship class occurs more than once, treat the fleet as invalid and output:

Ships not recognised

The game must not start.

Incorrect Ship Size

If a ship contains more or fewer coordinates than its required size, output:

Incorrect ship size

The game must not start.

Invalid Coordinate

If a ship contains a malformed coordinate or a coordinate outside the board, output:

Invalid coordinate

The game must not start.

Invalid Ship Position

If a ship's coordinates are not all on one horizontal or vertical line, output:

Ships cannot sit diagonally on the board

The game must not start.

If a ship is horizontal or vertical but its cells are not contiguous, treat this as invalid ship positioning and output the same message:

Ships cannot sit diagonally on the board

The game must not start.

Overlapping Ships

If two or more ships occupy the same coordinate, output:

Ships cannot overlap

The game must not start.

Multiple Errors

If the file contains more than one validation error, reporting the first error encountered is sufficient.

Do not start the game after any startup validation error.

Valid Configuration

If the file is valid, output:

Battleships Begin

The game can then start accepting player input.

Player Input

While the game is running, accept board coordinates such as:

A1
B3
J10

Coordinate input must be case-insensitive.

If a coordinate is malformed or outside the board, output:

Invalid coordinate

The player must then be allowed to try again.

If the player enters a coordinate that has already been entered, output:

Cannot enter same coordinate twice

The player must then be allowed to try again.

A repeated coordinate must not cause any additional hit to be recorded against a ship.

If the user presses the Escape key, immediately end the game.

Escape must not be treated as coordinate input.

Shot Results

For each new valid coordinate entered by the player:

If no ship occupies the coordinate, output:
Miss
If a ship occupies the coordinate and that ship still has one or more un-hit cells remaining after the shot, output:
Hit
If the shot hits the final un-hit cell belonging to that ship, output:
Sunk

Each coordinate can only be successfully fired upon once.

When every coordinate belonging to every ship has been hit, output:

Game Over

The application must then end the game.

If the final shot both sinks the final remaining ship and completes the game, output:

Sunk

followed by:

Game Over
```

### Finally up and running
The solution builds.
\
Unit tests are discoverable, runnable, and passing.
\
The game runs as expected, including:
- Load the input file at startup
- Handle user-input and console output during the game
- Exit the game if user hits `esc`
- Complete the game when all ships are sunk
