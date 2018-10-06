# Sheets
This is an API that matches the version 4 of [Google's sheets API](https://developers.google.com/sheets/api/reference/rest/) for the most part. So if you'd like documentation on how to use this API, you have to consult their API instead, and use the C# bindings.
There is also a smaller high level layer on top, which is useful for people who just need to access the data from a 2D array.

## Dependencies
- UnityEngine
- .NET Framework 4.5

## Removing the UnityEngine.dll dependency
To remove this dependency, the SheetsClient contains two methods for serializing and deserializing json data. These methods can be rewritten to use any other JSON API, like Newtonsoft.Json.

## High level API
The requirements to using the Google Sheets API, is to have the spreadsheetId and an access token. The spreadsheetId can be retrieved from a url.

`https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}`

The API token can be created from the Google API Console, for more info on this, visit [this article by google](https://cloud.google.com/docs/authentication/api-keys)

For both the low level and high level, a `SheetsClient` instance is needed, this is in the `Popcron.Sheets` namespace. The constructor takes the spreadsheetId and the access token.

```cs
public async void Start()
{
    string spreadsheetId = "";
    string token = "";
    SheetsClient client = new SheetsClient(spreadsheetId, token);
    Spreadsheet spreadsheet = await client.Get();

    Debug.Log("URL: " + spreadsheet.URL);
    Debug.Log("Title: " + spreadsheet.Title);
    Debug.Log("Rows: " + spreadsheet.Sheets[0].Rows);
    Debug.Log("Columns: " + spreadsheet.Sheets[0].Columns);
    
    Cell[,] data = spreadsheet.Sheets[0].Data;
}
```

If you'd like to use the low level API, you can use the `GetRaw()` method instead of `Get()`. The raw method alternative will out `SpreadsheetRaw` which is identical to the Google API reference.

## FAQ
- Does the BatchUpdate method work? A: Untested, I don't know.
- What about the Create method? A: Untested.
- What works? A: The Get method, and most of the high level and low level api.
- GetRaw doesn't return any grid data!! A: Use the hidden `includeGridData` parameter.
- I'm using the low level API and I don't know what X does? A: Look at the Google Sheets API, because neither do I.
