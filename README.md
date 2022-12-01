# HEPSİBURADA CHALLENGE

30.11.2022 
Burcu Nihal YAVUZ

## NASIL ÇALIŞTIRILIR 

| Gereksinim | Version  |
|------------|----------|
| Docker     | 20.10.14 | 
| Dotnet     | 6.0.101  |



- `docker-compose.yml` dosyası ile aynı klasördeyken aşağıdaki komutu çalıştırınız
 ```
docker-compose up -d
```


- db'den kontrol etmek isterseniz çalışan mongo instance'ına bağlanıp `test` veritabanını ve `products` ve `categories`  collectionını oluşturunuz

- api klasörü içerisinde önce
 ```
dotnet restore
```

sonrada 
 ```
dotnet run
```

komutlarını çalıştırarak API projesini çalıştırınız

- `apiURL /swagger` adresinden api'ı test edebilirsiniz


![mimari özet](https://github.com/burcunihal/hepsiburadaChallenge/blob/main/arcdiag.png)
