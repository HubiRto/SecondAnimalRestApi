CREATE TABLE Animal
(
    IdAnimal    int           NOT NULL,
    Name        nvarchar(200) NOT NULL,
    Description nvarchar(200) NULL,
    Category    nvarchar(200) NOT NULL,
    Area        nvarchar(200) NOT NULL,
    CONSTRAINT Animal_pk PRIMARY KEY (IdAnimal)
);

INSERT INTO Animal (IdAnimal, Name, Description, Category, Area)
VALUES (1, 'Lion', 'A large carnivorous feline mammal', 'Mammals', 'Africa'),
       (2, 'Tiger', 'A large predatory feline mammal of Asia', 'Mammals', 'Asia'),
       (3, 'Eagle', 'A bird of prey with a massive hooked bill and long broad wings', 'Birds', 'North America'),
       (4, 'Shark', 'A long-bodied chiefly marine fish with a cartilaginous skeleton', 'Fish', 'Ocean'),
       (5, 'Komodo Dragon', 'The largest living species of lizard', 'Reptiles', 'Indonesia'),
       (6, 'Wombat', 'A short-legged, muscular quadrupedal marsupials', 'Marsupials', 'Australia'),
       (7, 'Polar Bear', 'A bear native largely within the Arctic Circle', 'Mammals', 'Arctic'),
       (8, 'Penguin', 'A flightless bird living almost exclusively in the southern hemisphere', 'Birds', 'Antarctica'),
       (9, 'Anaconda', 'One of the largest and most powerful snakes in the world', 'Reptiles', 'South America'),
       (10, 'Kangaroo', 'A large herbivore native to Australia, famous for its powerful hind legs', 'Marsupials',
        'Australia');
