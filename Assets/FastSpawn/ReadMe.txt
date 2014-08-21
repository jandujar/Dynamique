Fast Spawn v1.1.1
=================

Fast Spawn allows the game to spawn game objects without any significant loss in performance. It also gives you an easy way to manage and access your prefabs. The high performance spawning is based on pooling, i.e. keeping a user selected amount of pre-instantiated/deactivated game objects in memory. Pooling will also greatly reduce the amount of required garbage collection which can can cause random lags in games. The spawn objects may be easily divided into groups and the groups can be selectively loaded and unloaded during level/scene changes or even during the gameplay.

Documentation
=============

Documentation is provived as a PDF document inside the FastSpawn folder.

Website
=======

See online documentaion and more about the Fast Spawn at http://www.pmjo.org/fastspawn

Support
=======

Incase you are having problems with Fast Spawn, don't hesitate to contact support@pmjo.org

Version history
===============

1.1.1:

- Library was rebuilt to support Windows Phone

1.1:

- A new example scene, BogeyManCannon
- New function (GetAvailableCount) for getting the count of inactive objects for FastSpawnObjects of IfAvailable type. Returns 0 for FastSpawnObjects of Always type.
- PDF documentation

1.0:

- Initial Asset Store version