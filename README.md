# ARBlockbot
An augmented reality experience for simplified programming and robotics education. Built by Kevin Feng, Anne Liu, Daniel Martin, and Anjali Thatte.

## To run:
  1. make sure you're on the default branch, and clone or download repo
  2. open project in Unity
  3. build and run Assets/Scenes/ARBlockbot on your Android device


## Basic instructions:
   - open menu
   - tap on any of the categories to access blocks
   - drag blocks onto coding area
   - snap them together
   - collapse the coding space and tap anywhere on plane to place robot
   - tap teal play button to execute code starting from the Start block
   - tap red stop button to terminate execution


## Basic maneuvers to try:

### Go forward:
   - Start
   - setThrottle(n), n is a percent of the max throttle

### Go in a circle:
   - Start
   - setThrottle(n)
   - setSteering(+/-m), m is a percent of max turn. +m is right turn, -m is left turn

### Make a turn:
   - Start
   - setThrottle(n)
   - setSteering(+/-m)
   - sleep(s), s is how many seconds to keep running previous command
   - setSteering(0)
