#Örnek Dynamic Sorgu Json Örnekleri
```
{
  "sort": [
    {
      "field": "DailyPrice",
      "dir": "desc"
    }
  ],
  "filter": {
    "field": "DailyPrice",
    "operator": "lt",
    "value": "2000",
    "logic": "and",
    "filters": [
      {
        "field": "Fuel",
        "operator": "eq",
        "value": "2",
        "logic": "and",
        "filters": [
          {
            "field": "Name",
            "operator": "contains",
            "value": "R",
            "logic": "and",
            "filters": [
              {
                "field": "Transmission.Name",
                "operator": "eq",
                "value": "Otomatik"
              }
            ]
          }
        ]
      }
    ]
  }
}
```
```
{
  "sort": [
    {
      "field": "DailyPrice",
      "dir": "desc"
    }
  ],
  "filter": {
    "field": "DailyPrice",
    "operator": "lt",
    "value": "2000"
  }
}
```
