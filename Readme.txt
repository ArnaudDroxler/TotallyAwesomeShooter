Déscription du projet:
	Totally Awesome Shooter est un jeu de plateforme en 3D à la première personne. Le but du jeu est de finir les niveaux le plus rapidement possible.
	Pour ce faire, le joueur a à sa disposition :
		- Accélération grâce a des techniques avancées de déplacement
		- Contrôle aérien du personnage
		- Wall jumps
		- Rocket jumps
		- Checkpoints
		- Portes à ouvrir

Utilisation du projet:
	Lancer tas.exe

Touches par défaut:
	- WASD		bouger
	- Clic		tirer
	- Espace	sauter
	- k		recommencer le niveau
	- r		suicide

Création de niveaux :
	Unity ne permettant pas de charger des scènes qui ne sont hors du build, voici la marche a suivre pour creer son propre niveau:
		- créer un nouvelle scene
		- il faut ajouter a cette scène au minimum un "GameManager" un "StartPlatform" et un "EndPlatform" (situés dans le dossier prefabs)
		- créer le reste du niveau à sa guise (Des plateformes sont a votre disposition dans le dossier prefabs)
		- Editer le script "UI_Manager_Script.cs" dans la méthode "StartLevelTuto" ou "StartLevel1", changer  le nom de la scène pour votre scène
		- Rebuild le programme
		- Enjoy

Tips and tricks: 
	- Maintenir la touche de saut en l'air permet de sauter instantanément dès que l'on touche le sol (Relacher entre chaque saut)
	- En l'air, le déplacement en diagonale permet de gagner de la vitesse
	- Pour faire un rocket jump, il faut sauter avant que la roquette n'explose