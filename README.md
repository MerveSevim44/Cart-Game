<h1>Strategic Card Warfare</h1>
<p>Welcome to the <strong>Card-Game</strong> repository! This project is a card-based game developed in C# using WPF
    (Windows Presentation Foundation). The game involves strategic card battles between a player and a computer, with
    various types of cards representing different military vehicles (air, land, and sea). The game features a
    user-friendly interface, sound effects, and a logging system to track game progress.</p>

<h2>Features</h2>
<ul>
    <li>
        <p><strong>Card Types</strong>: The game includes three main types of cards: Air, Land, and Sea. Each type has
            unique attributes and advantages over the others.</p>
    </li>
    <li>
        <p><strong>Gameplay</strong>: Players select cards to battle against the computer. The outcome of each battle is
            determined by the cards' attributes and their respective advantages.</p>
    </li>
    <li>
        <p><strong>Sound Effects</strong>: The game includes sound effects for card selection, battles, and game
            completion.</p>
    </li>
    <li>
        <p><strong>Logging System</strong>: The game logs all actions and results to a text file, which can be reviewed
            after the game ends.</p>
    </li>
    <li>
        <p><strong>User Interface</strong>: The game features a visually appealing interface with custom styles for
            buttons and combo boxes.</p>
    </li>
</ul>
<h2>How to Play</h2>
<ol start="1">
    <li>
        <p><strong>Start the Game</strong>: Launch the application and enter your name and ID to start the game.</p>
    </li>
    <li>
        <p><strong>Select Cards</strong>: Choose up to 3 cards from your hand to battle against the computer.</p>
    </li>
    <li>
        <p><strong>Battle</strong>: The game will simulate the battle between your selected cards and the computer's
            cards. The results will be displayed, and the game will log the outcomes.</p>
    </li>
    <li>
        <p><strong>Continue</strong>: After each round, new cards will be added to your hand, and you can continue
            playing until all rounds are completed.</p>
    </li>
    <li>
        <p><strong>Game Over</strong>: The game will display the final scores and determine the winner based on the
            total points accumulated.</p>
    </li>
</ol>
<h2>Screenshots</h2>
<img src="https://raw.githubusercontent.com/MerveSevim44/Cart-Game/refs/heads/main/Screenshots/Screenshot_2.png" alt="Screenshot 1" />
<img src="https://raw.githubusercontent.com/MerveSevim44/Cart-Game/refs/heads/main/Screenshots/Screenshot_1.png" alt="Screenshot 2" />
<h2>Installation</h2>
<ol start="1">
    <li>
        <p>Clone the repository to your local machine:</p>
        <div class="md-code-block">
            <div class="md-code-block-banner-wrap">
                <div class="md-code-block-banner">
                    <div class="md-code-block-infostring">bash</div>
                    <div class="md-code-block-action">
                        <div class="ds-markdown-code-copy-button">Copy</div>
                    </div>
                </div>
            </div>
            <pre><span class="token function">git</span> clone https://github.com/MerveSevim44/Card-Game.git</pre>
        </div>
    </li>
    <li>
        <p>Open the solution file <code>CardClash.sln</code> in Visual Studio.</p>
    </li>
    <li>
        <p>Build the solution to restore dependencies and compile the project.</p>
    </li>
    <li>
        <p>Run the application to start the game.</p>
    </li>
</ol>
<h2>Requirements</h2>
<ul>
    <li>
        <p><strong>.NET Framework 4.7.2</strong>: Ensure you have the .NET Framework 4.7.2 installed on your machine.
        </p>
    </li>
    <li>
        <p><strong>Visual Studio</strong>: The project is developed using Visual Studio 2019 or later.</p>
    </li>
</ul>
<h2>File Structure</h2>
<ul>
    <li>
        <p><strong>CardClash/</strong>: Contains the main game logic, XAML files, and resources.</p>
        <ul>
            <li>
                <p><strong>App.xaml</strong>: The main application file.</p>
            </li>
            <li>
                <p><strong>MainWindow.xaml</strong>: The main window where the game starts.</p>
            </li>
            <li>
                <p><strong>Game.xaml</strong>: The game window where the battles take place.</p>
            </li>
            <li>
                <p><strong>Cards/</strong>: Contains the classes for different types of cards (Air, Land, Sea).</p>
            </li>
            <li>
                <p><strong>Properties/</strong>: Contains application settings and resources.</p>
            </li>
            <li>
                <p><strong>image/</strong>: Contains images used in the game.</p>
            </li>
            <li>
                <p><strong>sounds/</strong>: Contains sound effects used in the game.</p>
            </li>
        </ul>
    </li>
    <li>
        <p><strong>README.md</strong>: This file, providing an overview of the project.</p>
    </li>
</ul>
<h2>Contributors</h2>
<ul>
    <li>
        <p><strong><a href="https://github.com/MerveSevim44" target="_blank" rel="noreferrer">MerveSevim44</a></strong>
        </p>
    </li>
    <li>
        <p><strong><a href="https://github.com/MYounesEG" target="_blank" rel="noreferrer">MYounesEG</a></strong></p>
    </li>
</ul>
<p>Enjoy the game! 🎮</p>
