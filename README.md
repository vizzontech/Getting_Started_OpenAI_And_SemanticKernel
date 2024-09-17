# Getting_Started_OpenAI_And_SemanticKernel

Clone repo 

Add dependencies  (Tools => NuGet Package Manager => Package Manager Console => run following.)
```
Install-Package Microsoft.Extensions.DependencyInjection
Install-Package Microsoft.Extensions.Logging
Install-Package Microsoft.Extensions.Logging.Console
Install-Package Microsoft.SemanticKernel

```


Build and run project 

Once you see the screen with the prompt "Q:", you can start interacting with the speaker data by trying out the following queries:

1. Find speakers: This should return a list of all available speakers.
2. Tony Robbins: This should return the details for the speaker "Tony Robbins."
3. id = 2 or id is 2: This should return the speaker with ID 2, which is "Tony Robbins."
4. Search for "motivational speaker" or "television host": This should return data based on the search terms, retrieving speakers that match those descriptions.

This will allow you to explore the speaker dataset interactively.









