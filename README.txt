FRENCH Version of the readme file (for now)

---------Flappy Bird---------
FlappyBird est un jeu très populaire ayant vu le jour il y a déjà un bon nombre d'années. C'est un jeu à la base simple qui peut être reproduit assez facilement, ce qui en fait un très bon choix pour s'entraîner à utiliser Unity2D.
De plus, cela fait suite à un tutorial sur AngryBird dans le même thème.

---------Qu'est-ce qui a été fait?---------
La première version visait à faire avancer notre protagoniste mais celle-ci fût vite abandonner à cause d'un départ à tâtons catastrophique.

- Dès lors, les tuyaux sont devenu mobile et notre protagoniste statique (simple choix subjectif)
- Il est évidant que notre petit oiseau peut battre des ailes et il gagne également une très légère accélération en se laissant tomber
- Une UI est mise en place pour les élément de base du jeu (Pause, score, titre écran de départ et de fin) ainsi qu'un ajout tardif d'un système de pièce

Le jeu est contituer d'une unique scène et d'animations simples.



---------Les points positifs---------
Difficulté cohérante pour un premier projet. Amélioration en ce qui concerne :
- L'utilisation des éléments d'UI
- L'utilisation des [SerializableField] ainsi que l'assignation de références
- Le code est de plus en plus propre, utilisation des balises #region, des //commentaires
- Compréhension général de la logique d'Unity2D


---------Les points négatifs---------
Cet entrainement est terminé mais certain points sont à noter pour être travailler sur d'autres projets:
- Les Enumérations: Clairement mal utilisé dans ce projet, à revoir.
- Les Co-routines: Utilisation correct malgrès des difficultés à comprendre leur interraction avec la fonction Update() et les boucles, à peaufiner.
- Optimization du code: Plus le projet avançait, plus le code devenait un empilement de pansements sur des pansements. Correct pour un premier projet mais à surveiller de près.
- Scriptable Object: Non utilisé ici mais aurait probablement pu le faire. Element important dans le projet suivant, à surveiller.
- TROP DE VALEURS CODE EN DUR. Va de pair avec les pansements énoncé plus haut.
- Difficulté à savoir ce qui est normal ou non dans la manière de coder, quelles variables assigner ou non, class à créer ou pas, ranger les fonctions dans les bons script
- Incompréhension des types Public et Private.
