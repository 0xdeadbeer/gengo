
# osu!gengo

Welcome to osu!gengo, a brand new ruleset (game mode) for osu! that combines the excitement of rhythm games with the power of language learning through [Anki](https://apps.ankiweb.net/). 
Get ready to gamify your language learning experience like never before!

## Gameplay Overview

Players will be challenged to improve their language skills by recognizing vocabulary from their own Anki decks and play at the beat of the song at the same time. 
By incorporating Anki, a powerful, intelligent, and open-source flash card system, we have created a unique synergy that allows players to learn and reinforce 
vocabulary while enjoying the fast-paced gameplay of osu!.

## Getting Started

To start your language learning journey with osu!gengo, follow these steps:
 - Download osu!lazer (obviously)
 - Download and install Gengo.dll (conveniently provided in the Releases tab of this repository)
 - Download Anki (Although we assume you already know what Anki is, how it works, and have a working deck that you regularly look after)
 - Download our [custom fork of Anki Connect](https://github.com/0xdeadbeer/anki-api) designed to work along osu!gengo and fascilitate problem solving on the API side in the future 

### osu!gengo settings

At the moment, there's 4 different settings for osu!gengo (2+ for input buttons).

 - `Anki URL (API)` Link of the Anki Connect API - default value is `http://localhost:8766`
 - `Anki Deck` **EXACT** name of the deck that you want to be reviewing/studying through this ruleset. 
 - `Foreign Word Field` **EXACT** name of the field meant for foreign words in the deck
 - `Translated Word Field` **EXACT** name of the field meant for translated words in the deck

## Feedback 

If you have any suggestions, bug reports, or general comments about osu!gengo, please let us know. 
Your feedback will help us improve the ruleset and create a better language learning experience for everyone. 

To submit feedback, you can: 
 - **Create an issue:** Visit our GitHub repository and create an issue detailing your feedback. Make sure to provide clear descriptions and, if applicable, any steps to reproduce the issue.
 - **Join the Discussion:** Engage with the osu!gengo community on our official GitHub repository. Participate in discussions, share your thoughts, and collaborate with other users to enhance the project.
 - ~~**Ask in the community server**~~ (if this reaches a big community, we will eventually consider opening up a Matrix/Discord server)

## Contributions and Development

We welcome contributions from the community. If you have programming skills, game design ideas, or language expertise, you can contribute to the development and improvement of osu!gengo. 

Steps to setup a development environment for osu!gengo:
 - Clone [osu!lazer](https://github.com/ppy/osu) and make sure you you can run a local version of it on your machine (that requires the necessary dotnet version, etc.)
 - Clone this repository and move the ruleset folders (`osu.Game.Rulesets.Gengo` & `osu.Game.Rulesets.Gengo.Tests`) into the osu!lazer folder 
 - Add the osu!gengo's folders to the project's .sln file (`osu.sln`). Usually done by executing the following example command `dotnet sln add NAME_OF_THE_FOLDER_TO_ADD`
 - Add the `osu.Game.Rulesets.Gengo/osu.Game.Rulesets.Gengo.csproj` project as a `<ProjectReference>` in the `osu.Desktop/osu.Desktop.csproj` file
 - Compile & Have fun!

## License

osu!gengo is released under the MIT License, which grants users the freedom to use, modify, and distribute the software. However, it comes with no warranty or guarantee of support. 
The user shall refer to the `LICENSE` file for more details.

