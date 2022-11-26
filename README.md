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

## Scenariusz
1. Użytkownik nawiązuje kontakt z botem.
2. Bot prosi o zdjęcie etykiety produktu.
3. Użytkownik przesyła zdjęcie.
4. Bot wysyła prośbę o wybranie pożądanej diety.
5. Użytkownik wybiera dietę
6. Bot wysyła wynik oceny, czy produkt jest odpowiedni dla wybranej diety


## Architektura
![diagram](https://raw.githubusercontent.com/Kacpu/diet-bot/master/img/architecture.png "diagram")

Bot Service wykorzystuje CosmosDB do pobierania danych dotyczących diet oraz Cognitive Service (OCR) do pozyskiwania listy składników ze zdjeć.

## Demo
[Wideo na youtube]()
## Zespół
<ul>
<li>
<a href="https://github.com/Kacpu">Kacper Tarłowski</a>
<ul>
<li>implementacja bota</li>
<li>projektowanie działania</li>
</ul>
<li> 
<a href="#">Jakub Gromiński</a>
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
 <ul>
 </ul>
