# HEPSİBURADA CHALLENGE

30.11.2022 
Burcu Nihal YAVUZ

## NASIL ÇALIŞTIRILIR 

| Gereksinim | Version  |
|------------|----------|
| Docker     | 20.10.14 | 
| Dotnet     | 6.0.101  |

- ana klasör içerisindeki Api image'ını yükleyiniz 
```
docker load -i apiImage.tar
```

veya image'ı kendinizde oluşturabilirsiniz. Bunun için önce ` /api ` klasörüne gidiniz ve

```
docker build -t apiImage . 
```

komutunu çalıştırınız

- `docker-compose.yml` dosyası ile aynı klasördeyken aşağıdaki komutu çalıştırınız
 ```
docker-compose up -d
```


- çalışan mongo instance'ına bağlanıp `test` veritabanını ve `products` collectionını oluşturunuz

- `localhost/swagger` adresinden api'ı test edebilirsiniz