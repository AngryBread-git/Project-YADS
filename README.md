# YADS (Yet Another Dialogue System) is a text typing system for Unity. It prints one character at a time to a TextMeshProUGUI component. It uses Scriptable Objects for easy writing and styling of dialogue.

## Features
* Rich Text Tags - <b> <size> <color> etc.
* Events - Call events when the text is printed.
* Animation - Animate the text in various ways.
* Dialogue Parts - Easily set lines of dialogue, print speed, animation style, and more.

<img width="1280" alt="DSLabV2_14" src="https://github.com/user-attachments/assets/5806ae00-4299-47fa-a04c-84387ae55509">

## Requirements
* Unity Version 2021.3 or later.
* TextMeshPRO

## How to use
1. Download this project.
2. Copy the contents “/Assets/Scripts” to your own scripts.
3. You can now create your own Character Scriptable Objects by right-clicking, then selecting “Create > ScriptableObjects > CharacterSO”
4. You can now create your own Dialogue Scriptable Objects by right-clicking, then selecting “Create > ScriptableObjects > DialogueSO”

<img width="1279" alt="DSLabV2_13" src="https://github.com/user-attachments/assets/26f91766-eedb-47b5-b7e6-ff50da745c4a">

## Examples in the project.
* In “/Assets/Examples/ScriptableObjects/CharacterSOs” there are examples of Character Scriptable Objects. These are examples of characters. They contain a name and voice clip.
* In “/Assets/Examples/ScriptableObjects/DialogueSOs” there are examples of DialogueScriptable Objects. These are examples of dialogues, which in turn are made of DialogueParts. Each DialoguePart contains a line of text, a reference to the character that is saying the line, printing speed, settings for the animation, and events to fire before and after the line.
* In “/Assets/Examples/Scenes” there are 2 example scenes. Which showcase Dialogues, animations, events, and more. You can walk around with WASD. When a speech bubble appears above another character, you can press E to start a conversation. Press E again to continue to the next line. Some characters have more than one conversation available.
* In “/Assets/Examples/Scripts” there are some basic scripts that interact with the events included in the project. Such as SoundFromEvent which plays a sound when an event is fired. It also contains a basic character controller.
* These are examples and can of course be expanded on for the project they are used in.

<img width="1280" alt="DSLabV2_12" src="https://github.com/user-attachments/assets/b9205a58-58f6-4742-8f1d-862821db6e53">

## Credits for music and sounds.
Arcane Incantation by Keisuke Ito
Field of Hopes and Dreams by Toby Fox
Pleasant Porridge by Kevin MacLeod
Three Bar Logos by Jami Lynne.

Lime_addcounter by Rata & Jimbles
Other sounds by Atsushi Mori

  
