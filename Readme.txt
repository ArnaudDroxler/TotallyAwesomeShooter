Utilisation du projet:
	Lancer tas.exe

Création de niveaux :
	Unity ne permettant pas de charger des scènes qui ne sont hors du build, voici la marche a suivre pour creer son propre niveau:
		- créer un nouvelle scene
		- il faut ajouter a cette scène au minimum un "GameManager" un "StartPlatform" et un "EndPlatform" (situés dans le dossier prefabs)
		- créer le reste du niveau à sa guise (Des plateformes sont a votre disposition dans le dossier prefabs)
		- Editer le script "UI_Manager_Script.cs" dans la méthode "StartLevelTuto" ou "StartLevel1", changer  le nom de la scène pour votre scène
		- Rebuild le programme
		- Enjoy