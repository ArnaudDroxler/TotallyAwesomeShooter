Utilisation du projet:
	Lancer tas.exe

Cr�ation de niveaux :
	Unity ne permettant pas de charger des sc�nes qui ne sont hors du build, voici la marche a suivre pour creer son propre niveau:
		- cr�er un nouvelle scene
		- il faut ajouter a cette sc�ne au minimum un "GameManager" un "StartPlatform" et un "EndPlatform" (situ�s dans le dossier prefabs)
		- cr�er le reste du niveau � sa guise (Des plateformes sont a votre disposition dans le dossier prefabs)
		- Editer le script "UI_Manager_Script.cs" dans la m�thode "StartLevelTuto" ou "StartLevel1", changer  le nom de la sc�ne pour votre sc�ne
		- Rebuild le programme
		- Enjoy