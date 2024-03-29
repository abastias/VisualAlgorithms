# Visual Algorithms
Seeing an execution of any algorithm in visual form allow us a better understanding of the advantages and the disadvantages in the context of use. This project is completely open-souce. It was created using VSCode and Blazor/.NET Core 6 LTS, please use c# to code algorithms

## The project was created from an empty folder using the command
    dotnet new blazorserver -f net6.0
    
## To run locally this code, go to your VSCode terminal and run the command
    dotnet watch run
    
## Website
  The project has been deployed to azure web service. You can see the solution here:
  https://VisualAlgorithms.azurewebsites.net
    
  ![Alt text](wwwroot/images/KnightsTour.png?raw=true "The Knight's Tour")
    
# Algorithms
This project has few algorithms. Your help is needed.

## Sudoku
A puzzle in which players insert the numbers one to nine into a grid consisting of nine squares subdivided into a further nine smaller squares in such a way that every number appears once in each horizontal line, vertical line, and square.
We define a initial setup and the algorithm (Backtracking) will find a solution (if exists) 

<img src="/wwwroot/images/Sudoku.png" width="250">

## Pathfinder
Getting from point A to Point B is always a challenge. There are several algorithms to find the most efficient route. This is a first approach to findding a route using a simple algorithm that optimizes the minimum distance from the current position to the destination (Point B) of each step. This is not the best solution, but the purpose is to show an idea how to do it. 

<img src="/wwwroot/images/Pathfinder.png" width="350">

## Pi calculation using the Monte Carlo method
The method used square with a inside circle. The x-Axis and y-Axis goes from -1 to 1 and the radio for the inside circle is 1. In order to approach this method you can formulate the questions <i>what is the probability of a any random number for a pair (x,y) will be inside the circle?</i> The relation between randon inside pair to the ouside of the circle provides an approximation of the Pi number.

<img src="/wwwroot/images/Pi_Montecarlo.png" width="230">

## The Knight's Tour
A knight's tour is a sequence of moves of a knight on a chessboard such that the knight visits every square exactly once.
The knight's tour problem is the mathematical problem of finding a knight's tour. Creating a program to find a knight's tour is a common problem given to computer science students. Variations of the knight's tour problem involve chessboards of different sizes than the usual 8 × 8, as well as irregular (non-rectangular) boards.

<img src="/wwwroot/images/KnightTourAnimation.gif" style="float:right">


## Sort
A Sorting Algorithm is used to rearrange a given array or list elements according to a comparison operator on the elements. The comparison operator is used to decide the new order of elements in the respective data structure.

<img src="/wwwroot/images/Sort.png" width="300">

### Bubble Sort
Thi is simplest sorting algorithm that works by repeatedly swapping the adjacent elements if they are in the wrong order. 
This algorithm is not suitable for large data sets as its average and worst case time complexity is quite high.
### Selection Sort
The selection sort algorithm sorts an array by repeatedly finding the minimum element (considering ascending order) from unsorted part and putting it at the beginning.



  
# Contributors
  Your are invited to collaborate with this project to help other to understand algorithms.
  - Please pickup your favourite algorithms.
  - Create or improve a razor component.
  - Create a necesary interaction with input boxers and execution buttons.
  - Make sure the razor is accesible from the side menu bar.
  - Blazor has memory restriction, be aware if you use recursion.
  

