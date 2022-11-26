
# Diet Bot: bot dietetyczny
## Cel

Stworzenie bota służącego do
**klasyfikowania składników z etykiet produktów spożywczych według diety wybranej przez użytkownika**

Użytkownik dzięki temu oszczędza czas na wyszukiwaniu składników, których nie jest pewien, czy należą do konkretnej diety.


## Opis funkcjonalności

Bot umożliwia:
- prowadzenie rozmowy z użytkownikiem poprzez czat
- przesyłanie zdjęć
- rozpoznawanie tekstu ze zdjęć etykiet
- określenie, czy produkt jest odpowiedni dla wybranej diety

## Schemat działania
1. Użytkownik nawiązuje kontakt z botem.
2. Bot prosi o zdjęcie etykiety produktu.
3. Użytkownik przesyła zdjęcie.
4. Bot wysyła prośbę o wybranie pożądanej diety.
5. Użytkownik wybiera dietę
6. Bot wysyła wynik oceny, czy produkt jest odpowiedni dla wybranej diety


## Demo
[Wideo na youtube]()

## Architektura
![diagram](https://raw.githubusercontent.com/Kacpu/diet-bot/master/img/architecture.png "diagram")

Bot Service wykorzystuje CosmosDB do pobierania danych dotyczących diet oraz Cognitive Service (OCR) do pozyskiwania listy składników ze zdjeć.

## Opis wybranych serwisów
### Bot services
[**Bot Services**](https://azure.microsoft.com/pl-pl/products/bot-services/) - serwisy umożliwiające tworzenie bota w przy użyciu SDK w kilku językach (javascript, .NET , python). Pozwalają na tworzenie interaktywnego czatu z użytkownikiem.

Podczas tworzenia projektu wykorzystaliśmy wariant z użyciem .NET w wersji 6. 
### App services
[**App Services**](https://azure.microsoft.com/pl-pl/products/app-service/)

### Cosmos DB
[**Cosmos DB**](https://azure.microsoft.com/pl-pl/products/cosmos-db/) - szybka, rozproszona i nierelacyjna baza danych, która skaluje w zależności od obciążenia. Umożliwia zamieszczanie danych, jako zestawy "dokumentów". 

W projekcie była używana do zamieszczenia zestawu danych dostarczających informacji o dietach, oraz do pozyskiwania tych informacji przez bota.

### Cognitive Services

[**Cognitive Services**](https://azure.microsoft.com/pl-pl/products/cognitive-services) - to zbiór usług w obrębie chmurowej platformy Microsoft Azure opartych o sztuczną inteligencję, które w prosty sposób, czyli bez specjalistycznej wiedzy z zakresu AI oraz analizy danych, umożliwiają aplikacjom w pewnym sensie zyskanie zdolności widzenia, słyszenia, mowy czy analizy.

Podczas realizacji projektu wykorzystano część usługi związaną z rozpoznawaniem zdjęć, a mianowicie OCR -- Optical Character Recognition. Narzędzie to pozwala na odczytanie tekstu -- drukowanego lub pisanego ręcznie -- ze zdjęcia. W szczególności wykorzystane ono zostało do uzyskania listy składników z fotografii etykiety produktu spożywczego.

## Stos technologiczny

ASP.NET 6.0

## Zespół
<ul>
<li>
<a href="https://github.com/Kacpu">Kacper Tarłowski</a>
<ul>
<li>implementacja bota</li>
<li>projektowanie działania</li>
</ul>
<li> 
<a href="https://github.com/jgrominski">Jakub Gromiński</a>
<ul>
<li>research danych</li>
<li> przygotowanie cognitive service</li>
<li>projektowanie działania</li>
</ul>
 <li>
 <a href="https://github.com/BKopysc">Bartłomiej Kopyść</a>
 <ul>
 <li> przygotowanie Cosmos DB</li>
 <li>pozyskiwanie danych</li>
 <li>projektowanie działania</li>
 <li>przygotowanie grupy na Azure</li>
 <ul>
 </ul>
