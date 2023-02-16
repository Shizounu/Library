Creating a statemachine
    - Add the Statemachine Component to a GameObject
    - Rightclick in a folder navigate to "Create > Shizounu > AI > State" to create a state


To create an Action:
    - Create a new C# Script 
    - Inherint the script from "Shizounu.Library.AI.Action" 
    - implement the "Act" function to do what you need it to do
    
To create an Decision:
    - Create a new C# Script 
    - Inherint the script from "Shizounu.Library.AI.Decision" 
    - implement the "Act" function to do what you need it to do

Using the state editor:
    - This is a tool to more easily connect states and edit the actions once they are created
    - It gives you a more visual overview about how you conenct the states to make it easier to plan out the AI behaviour

    - You open the window by going to your tool bar and Open "Shizounu > Node Based Editor" 
    - You can add nodes by right clicking into the window
    - You can assign states to them using the reference window on the Note
    - If you have 2 states you can connect them by going from the output node (right) to the input node (left) of the state you want to transition into

    