# DevAI
A C#, ASP.NET API used to interact with a .NET AI.

This AI is based on [.NET Machine Learning](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai) and is currently trained to identify the difference between Cats and Dogs, spotting the sentiment of a comment or message (positive/negative) and whether two images are identical.

You can use the [demo site](http://s1.publiczeus.com:5165/) to play around with this or feel free to run it for yourself!

## Setup
This solution is build in Visual Studio 2022 and is built in .NET 6.

To host you MUST change the launch profile json in the properties folder to your external IP. You will see the "htt://s1.publiczeus.com" domain in that file, simply change this to your IP/domain and then you can build/launch the program.
