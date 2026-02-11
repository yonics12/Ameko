Ameko ist um Projekte herum konzipiert. Es ist zwar nicht notwendig, Projekte zu verwenden, und du kannst auch ohne sie Untertitel bearbeiten und Ameko anderweitig nutzen, aber Projekte sind dafür gedacht, die Arbeit mit mehreren Untertiteldateien zu vereinfachen, gerade, wenn man mit anderen zusammenarbeitet.

## Der Projekt-Explorer

![](../assets/project-explorer-empty.png)

When you first open Ameko, the Project Explorer will be on the left, listing the currently-open documents. When a
project file isn't loaded, the Default Project serves as a dumping ground for the files you open during the session. You
can save the project to a file if you want to leverage the benefits of using a project file.

## Öffnen eines Ordners als Projekt

Wenn du bereits eingerichteten Projektordner hast, kannst du die Struktur in Ameko importieren, indem du den Ordner als Projekt öffnest.
Dadurch werden alle Unterordner und Untertiteldateien in den Projekt-Explorer geladen, wo du den Inhalt anpassen und das resultierende Projekt als Datei speichern kannst.

## Display Names and You

Zwar _können_ Projektstruktur und -namen den Dateien auf deiner Festplatte entsprechen, jedoch kannst du sie auch problemlos neu ordnen und umbenennen, ohne Auswirkung auf die zugrundeliegenden Dateien. Betrachte beispielsweise die folgende flache Hierarchie mit wortreichen Dateinamen:

```
Kono Bijutsubu ni wa Mondai ga Aru/
  [AMK] Konobi - 01 - Dialogue.ass
  [AMK] Konobi - 01 - Typesetting1.ass
  [AMK] Konobi - 01 - Typesetting2.ass
  Konobi - 01 - Captions.ja.srt
  [AMK] Konobi - 02 - Dialogue.ass
  [AMK] Konobi - 02 - Typesetting1.ass
  [AMK] Konobi - 02 - Typesetting2.ass
  Konobi - 02 - Captions.ja.srt
```

Mithilfe von Anzeigenamen kann man das innerhalb des Projektes neu organisieren und aufräumen, ohne die vorhandenen Dateien zu beeinflussen:

```
01/
  Dialogue.ass
  TS1.ass
  TS2.ass
  Captions.srt
02/
  Dialogue.ass
  TS1.ass
  TS2.ass
  Captions.srt
```

## Project Configuration

![](../assets/project-config.png)

One of the key benefits of using Projects when working in a team is synced configuration. Options set in the Project
Configuration will override user preferences while the project is loaded. This is great for keeping everyone's CPS warn
threshold is the same, and critically, keeping a shared spellcheck dictionary and making sure everyone is using the same
spellcheck language. If the project is set to use English (GB), for example, _everyone_ will be using English (GB), and
that extra u in colour won't be missed.

![](../assets/project-install-dictionary.png)

Users will be prompted to download the appropriate dictionary if they don't already have it.

![](../assets/spellcheck.png)

Words can be added to the project dictionary directly from the spellchecker.

## Git Integration

![](../assets/git-toolbox.png)

When saved in the root of a project (next to the `.git` directory), Project files allow for easy access to basic Git
functions, like commiting, pushing, pulling, and viewing a list of recent commits.
