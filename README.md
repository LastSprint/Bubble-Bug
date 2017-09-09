
![license](https://img.shields.io/github/license/mashape/apistatus.svg)
![Build Status](https://img.shields.io/circleci/project/github/RedSparr0w/node-csgo-parser.svg)
![Conda](https://img.shields.io/conda/pn/conda-forge/python.svg)

# Bubble-Bug -  Tools for generating mock server from Apiary specification.
# Утилита для генерации мокового сервера из специйикации ApiBlueprint.
## В данный момент сервер поддерживает:
+ POST/PUT/DELETE/GET - запросы
+ Горячую подмену моков
+ Сравнение по параметрам и телу запроса
+ Ответ с нужным http кодом
+ Запросы без параметров и тела
+ Запросы только с параметрам
+ Запросы только с телом
+ Запросы и с параметром и с телом
+ Для ответом аналогично

## Как это работает
+ -> Формируется запрос.
+ -> Сервер обрабатывает запрос.
+ -> Получает из него параметры и тело.
+ -> Из директории запроса считываются все файлы и парсятся во внутренний объект. 
+ -> Принятый запрос поочередно сравнивается с моковыми - проверяется равенство параметров, затем равенство тел запросов.
+ -> Если нужный мок был найден - возвращается тело ответа из мока, иначе возвращается ошибка. 

## Структура мокового файла
```
{
    "ResponseBody": {
        "access_token": "some_access_token",
        "refresh_token": "some_refresh_token"
    },
    "ResponseCode": 200,
    "ResponseHeaders": null,
    "Parameters": [
        {
            "Type": 1,
            "ValueType": 2,
            "Value": "some_id",
            "Name": "client_id"
        },
        {
            "Type": 1,
            "ValueType": 2,
            "Value": "authorization_code",
            "Name": "grant_type"
        },
        {
            "Type": 0,
            "ValueType": 2,
            "Value": "secretCde",
            "Name": "code"
        }
    ],
    "RequestHeaders": null,
    "RequestBody": null
}
```
Где 
+ `ResponseBody` - Тело ответа. Именно этот объект вернется в качестве ответа сервера.
+ `ResponseCode` - Код ответа.
+ `ResponseHeaders` - **Пока не работает**
+ `Parameters` - Параметры, которые ожидается получить в запросе.
+ `Parameter.Type` - 0 - Optional, 1 - Required
+ `Parameter.ValueType`
  - Object - 0
  - String - 1
  - Number - 2
  - Bool - 3
+ `Value` - Значение параметра
+ `Name` - Имя параметра
