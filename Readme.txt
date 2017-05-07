D�scription du projet:
	Totally Awesome Shooter est un jeu de plateforme en 3D � la premi�re personne. Le but du jeu est de finir les niveaux le plus rapidement possible.
	Pour ce faire, le joueur a � sa disposition :
		- Acc�l�ration gr�ce a des techniques avanc�es de d�placement
		- Contr�le a�rien du personnage
		- Wall jumps
		- Rocket jumps
		- Checkpoints
		- Portes � ouvrir

Utilisation du projet:
	Lancer tas.exe

Touches par d�faut:
	- WASD		bouger
	- Clic		tirer
	- Espace	sauter
	- k		recommencer le niveau
	- r		suicide

Cr�ation de niveaux :
	Unity ne permettant pas de charger des sc�nes qui ne sont hors du build, voici la marche a suivre pour creer son propre niveau:
		- cr�er un nouvelle scene
		- il faut ajouter a cette sc�ne au minimum un "GameManager" un "StartPlatform" et un "EndPlatform" (situ�s dans le dossier prefabs)
		- cr�er le reste du niveau � sa guise (Des plateformes sont a votre disposition dans le dossier prefabs)
		- Editer le script "UI_Manager_Script.cs" dans la m�thode "StartLevelTuto" ou "StartLevel1", changer  le nom de la sc�ne pour votre sc�ne
		- Rebuild le programme
		- Enjoy

Tips and tricks: 
	- Maintenir la touche de saut en l'air permet de sauter instantan�ment d�s que l'on touche le sol (Relacher entre chaque saut)
	- En l'air, le d�placement en diagonale permet de gagner de la vitesse
	- Pour faire un rocket jump, il faut sauter avant que la roquette n'explose