use ZooDemo
go

insert into tbPavilon values('jedna')
insert into tbPavilon values('jedna')
insert into tbPavilon values('jedna')
insert into tbPavilon values('jedna')

insert into tbAnimalType values('Savci', 'info o savci')
insert into tbAnimalType values('Plazi', 'info o plazi')
insert into tbAnimalType values('Ryby', 'info o ryby')
insert into tbAnimalType values('Pt�ci', 'info o pt�ci')

insert into tbAnimal values('Ko?ka dom�c�', getdate() - rand()* 1000, 1,1, 'kocka.png')
insert into tbAnimal values('Varan komodsk�', getdate()- rand()* 1000, 2,2, 'varan.png')
insert into tbAnimal values('�tika drav�', getdate()- rand()* 1000, 3,3, 'stika.png')
insert into tbAnimal values('S�kora ko?adra', getdate()- rand()* 1000, 4,4,'sykora.png')