Si Ahmed Haddi Yannis SIAY05030300  
Eraud Florentin ERAF04070000  
Pour la bonne execution du projet, il est nécessaire d'avoir l'application Docker Desktop installée et lancée (il est nécessaire d'avoir le docker engine en cours pour que le programme puisse s'exécuter).
Lancer le script powershell RunAll.ps1  
Trouver le terminal en avec les écritures en blanc, CommandSource, qui a les lignes "Automatic : ", il s'agit du service ou vous pouvez lancer les commandes manuellement en vous inspirant du modèle.  
Il est également possible de lancer une deuxième instance de CommandSource pour lancer des commandes en parallèle.  C'est également possible pour les autres micro services mais, cela n'as pas d'intérêt.  
Il est également possible de fermer des services sans que cela ne ferment les autres services, cependant le service de Validation est responsable de la transmission aux différents processors, ils ne pourront donc plus recevoir de commande.  

