# GMTK GameJam 2025

Project Repo for the GMTK GameJam 2025

Idee:
* Stealth Game
  * Übersicht
    * Wächter, Kameras etc in festen Bewegungsabläufen
    * Kameras durchschalten (markierung), bewegte Kameras, Line of Sight bei Licht?
    * Schalter für Türen, Licht, etc
    * Kameraschalter
  * Aufdecken der Karte (?)
    * Eher als extra Modus, vermutlich eher nervig
  * Keine Echtzeit, Liste an Befehlen (Laufen, Interagieren, Dodge Roll)
    * Laufe nach Links, Rechts, Oben, Unten für 
* Rogue-Like
  * Klassiker wie Binding of Issac
  * Nach jeder Ebene wird der Loot wieder entfernt
  * Ziel ist die Räume zu beobachten, um in einem Durchlauf die Räume in der richtigen Reihenfolge zu betreten
  * Die Räume sind trotzdem alle zufällig, nur der versteckte Pfad ist fest und kann visuell (irgendwie) erahnt werden
* Simples Handy-Game mit nur 1-2 Knöpfen mit Jump&Run
  * Gameplay ähnlich wie https://store.steampowered.com/app/3448750/Frog_Jump/
  * Level baut sich nach und nach generisch auf und Pfad ist auf einem Kreis
  * Altes Level wird also nach und nach überschrieben
  * Ziel ist einfach so viele Runden zu schaffen wie möglich
  * Wenn man runterfällt ist der Run vorbei

---

Name: WIP Stealth Game

GameLoop: Spieler gibt die Aktonen des Loop vor um ans Ende der Karte zu navigieren, dabei muss er Hindernisse überwinden.

UI: Links Loop, Rechts Karte, oben mitte Ticks (ähnlich Playbutton?)

Ticks
- Zeiteinheiten im Spiel quasi Züge

Aktionen: Vom Spieler ausgewählte Aktionen die dem Loop hinzugefügt werden, eine Aktion dauert Ticks
- Warten (Dauer)
- Laufen (Richtung, Schrittzahl)
- Objekt betätigen

Loop:
- Reihenfolge in der Aktionen ausgeführt werden

Hindernisse:
- Wachen, haben Sichtweite, Geschwindigkeiten, feste Bewegungsabläufe
- Schalter -> öffnet Türen, deaktiviert Kameras, Licht etc.
- Licht
- Kameras

Sonstiges:
- Bronze, Silber, Gold je nachdem wie viele Ticks genutzt wurden
- Collectable pro Level
