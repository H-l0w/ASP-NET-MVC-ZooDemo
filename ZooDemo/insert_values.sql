use ZooDemo
go

insert into tbPavilon values('jedna')
insert into tbPavilon values('jedna')
insert into tbPavilon values('jedna')
insert into tbPavilon values('jedna')

insert into tbAnimalType values('Savci', 'info o savci')
insert into tbAnimalType values('Plazi', 'info o plazi')
insert into tbAnimalType values('Ryby', 'info o ryby')
insert into tbAnimalType values('Ptáci', 'info o ptáci')

insert into tbAnimal values('Ko?ka domácí', getdate() - rand()* 1000, 1,1, 'kocka.png')
insert into tbAnimal values('Varan komodský', getdate()- rand()* 1000, 2,2, 'varan.png')
insert into tbAnimal values('Štika dravá', getdate()- rand()* 1000, 3,3, 'stika.png')
insert into tbAnimal values('Sýkora ko?adra', getdate()- rand()* 1000, 4,4,'sykora.png')