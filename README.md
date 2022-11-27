 # Diet Bot: bot dietetyczny
 * [Cel](#cel)
  * [Opis funkcjonalności](#opis-funkcjonalności)
  * [Schemat działania](#schemat-działania)
  * [Architektura](#architektura)
  * [Demo](#demo)
  * [Opis wybranych serwisów](#opis-wybranych-serwisów)
    + [Bot services](#bot-services)
    + [App services](#app-services)
    + [Cosmos DB](#cosmos-db)
    + [Cognitive Services](#cognitive-services)
  * [Stos technologiczny](#stos-technologiczny)
  * [Zespół](#zespół)


## Cel

Celem projektu było stworzenie bota służącego do **sprawdzania, czy składniki z etykiet produktów spożywczych są odpowiednie dla wybranej przez użytkownika diety**.

Użytkownik dzięki temu oszczędza czas na wyszukiwaniu składników, których nie jest pewien, czy należą do konkretnej diety.


## Opis funkcjonalności

Bot umożliwia:
- prowadzenie rozmowy z użytkownikiem poprzez czat,
- przesyłanie zdjęć,
- rozpoznawanie tekstu ze zdjęć etykiet,
- określenie, czy produkt jest odpowiedni dla wybranej diety.

## Schemat działania
1. Użytkownik nawiązuje kontakt z botem.
2. Bot wysyła prosbę o zdjęcie etykiety produktu.
3. Użytkownik przesyła zdjęcie.
4. Bot wysyła prośbę o wybranie pożądanej diety.
5. Użytkownik wybiera dietę.
6. Bot wysyła wynik oceny czy produkt jest odpowiedni dla wybranej diety.

## Architektura
![diagram](https://raw.githubusercontent.com/Kacpu/diet-bot/master/img/architecture.png "diagram")

Bot Service wykorzystuje Cosmos DB do pobierania danych dotyczących diet oraz Cognitive Service (OCR) do pozyskiwania listy składników ze zdjeć.

## Demo
[Wideo na youtube]()

## Opis wybranych serwisów

### Bot Services
[**Bot Services**](https://azure.microsoft.com/pl-pl/products/bot-services/) &#8211; umożliwia tworzenie środowisk konwersacyjnych w prostu sposób. Zapewnia zintegrowane środowisko programistyczne do tworzenia botów zarówno bez pisania kodu, jak i przy pomocy Bot Framework SDK (dostępne w kilku językach np. JavaScript, Python, C#).

Podczas projektu stworzono bota przy użyciu SDK dla ASP.NET Core 6 oraz testowano go przy użyciu Bot Framework Emulator.

### App Services
[**App Services**](https://azure.microsoft.com/pl-pl/products/app-service/) &#8211; oparte o HTTP serwisy przeznaczone do hostowania aplikacji internetowych, RESTowych API i back-endu aplikacji mobilnych. Umożliwiają pisanie kodu w szerokiej gamie języków programowania i technologii.

W projekcie serwis ten wykorzystano w celu upublicznienia kodu odpowiedzialnego za logikę aplikacji oraz komunikację między botem, bazą danych a Cognitive Services.

### Cosmos DB
[**Cosmos DB**](https://azure.microsoft.com/pl-pl/products/cosmos-db/) &#8211; szybka, rozproszona i nierelacyjna baza danych, która skaluje w zależności od obciążenia. Umożliwia zamieszczanie danych, jako zestawy "dokumentów". 

W projekcie była używana do zamieszczenia zestawu danych dostarczających informacji o dietach, oraz do pozyskiwania tych informacji przez bota.

### Cognitive Services
[**Cognitive Services**](https://azure.microsoft.com/pl-pl/products/cognitive-services) &#8211; zbiór usług w obrębie chmurowej platformy Microsoft Azure opartych o sztuczną inteligencję, które w prosty sposób, czyli bez specjalistycznej wiedzy z zakresu AI oraz analizy danych, umożliwiają aplikacjom w pewnym sensie zyskanie zdolności widzenia, słyszenia, mowy czy analizy.

Podczas realizacji projektu wykorzystano część usługi związaną z rozpoznawaniem zdjęć, a mianowicie OCR &#8211; Optical Character Recognition. Narzędzie to pozwala na odczytanie tekstu &#8211; drukowanego lub pisanego ręcznie &#8211; ze zdjęcia. W szczególności wykorzystane ono zostało do uzyskania listy składników z fotografii etykiety produktu spożywczego.

## Stos technologiczny

- ASP.NET Core 6.0
- Bot Framework SDK for C#

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
