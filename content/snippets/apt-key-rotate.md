+++
title = "apt-key-rotate"
date = 2026-10-02
description = "Rotate an apt key"
taxonomies = { tags = ["Bash", "Linux"] }
+++

It looks like HashiCorp had to rotate their GPG signing key for apt packages on 9/11/2026.  This served as a good reminder to me in rotating the key for apt packages:

```Shell
joelvandiver.github.io on  main [?] 
❯ sudo apt update
Get:1 https://cli.github.com/packages stable InRelease [3,917 B]
Hit:2 https://download.docker.com/linux/ubuntu resolute InRelease                                   
Get:3 https://apt.releases.hashicorp.com resolute InRelease [12.9 kB]                               
Err:3 https://apt.releases.hashicorp.com resolute InRelease                                         
  The following signatures couldn't be verified because the public key is not available: NO_PUBKEY FC9CA96ACA026560
Hit:5 http://us.archive.ubuntu.com/ubuntu resolute InRelease                                        
Hit:6 https://packages.microsoft.com/repos/edge-stable stable InRelease                             
Hit:7 https://packages.microsoft.com/repos/code stable InRelease                                    
Hit:8 http://us.archive.ubuntu.com/ubuntu resolute-updates InRelease                                
Hit:4 https://prod-cdn.packages.k8s.io/repositories/isv:/kubernetes:/core:/stable:/v1.36/deb  InRelease
Hit:9 http://us.archive.ubuntu.com/ubuntu resolute-backports InRelease
Hit:10 http://security.ubuntu.com/ubuntu resolute-security InRelease
Fetched 16.8 kB in 0s (33.7 kB/s)
23 packages can be upgraded. Run 'apt list --upgradable' to see them.
Warning: An error occurred during the signature verification. The repository is not updated and the previous index files will be used. OpenPGP signature verification failed: https://apt.releases.hashicorp.com resolute InRelease: The following signatures couldn't be verified because the public key is not available: NO_PUBKEY FC9CA96ACA026560
Warning: Failed to fetch https://apt.releases.hashicorp.com/dists/resolute/InRelease  The following signatures couldn't be verified because the public key is not available: NO_PUBKEY FC9CA96ACA026560
Warning: Some index files failed to download. They have been ignored, or old ones used instead.


❯ rg hashicorp /etc/apt/sources.list /etc/apt/sources.list.d/
/etc/apt/sources.list.d/hashicorp.list
1:deb [signed-by=/usr/share/keyrings/hashicorp-archive-keyring.gpg] https://apt.releases.hashicorp.com resolute main


joelvandiver.github.io on  main [?] 
❯ wget -O- https://apt.releases.hashicorp.com/gpg |
   sudo gpg --dearmor --yes -o /usr/share/keyrings/hashicorp-archive-keyring.gpg
--2026-10-03 12:09:52--  https://apt.releases.hashicorp.com/gpg
Resolving apt.releases.hashicorp.com (apt.releases.hashicorp.com)... 13.224.187.128, 13.224.187.116, 13.224.187.33, ...
Connecting to apt.releases.hashicorp.com (apt.releases.hashicorp.com)|13.224.187.128|:443... connected.
HTTP request sent, awaiting response... 200 OK
Length: 1725 (1.7K) [application/x-www-form-urlencoded]
Saving to: ‘STDOUT’

-                         100%[====================================>]   1.68K  --.-KB/s    in 0s    

2026-10-03 12:09:52 (140 MB/s) - written to stdout [1725/1725]


joelvandiver.github.io on  main [?] took 24s 
❯ gpg --no-default-keyring \
   --keyring /usr/share/keyrings/hashicorp-archive-keyring.gpg \
   --list-keys --keyid-format long
/usr/share/keyrings/hashicorp-archive-keyring.gpg
-------------------------------------------------
pub   rsa4096/FC9CA96ACA026560 2026-09-09 [SC] [expires: 2031-09-08]
      D55C0D1AC78A8D8126CB631CFC9CA96ACA026560
uid                 [ unknown] HashiCorp Security (HashiCorp Package Signing) <security+packaging@hashicorp.com>


> sudo apt update
Get:1 https://cli.github.com/packages stable InRelease [3,917 B]
Hit:2 https://download.docker.com/linux/ubuntu resolute InRelease                                   
Hit:3 https://packages.microsoft.com/repos/edge-stable stable InRelease                             
Hit:5 https://packages.microsoft.com/repos/code stable InRelease                                    
Get:6 https://apt.releases.hashicorp.com resolute InRelease [12.9 kB]                               
Hit:7 http://security.ubuntu.com/ubuntu resolute-security InRelease                                 
Hit:8 http://us.archive.ubuntu.com/ubuntu resolute InRelease                                        
Get:9 https://apt.releases.hashicorp.com resolute/main amd64 Packages [324 kB]                      
Hit:10 http://us.archive.ubuntu.com/ubuntu resolute-updates InRelease                               
Hit:4 https://prod-cdn.packages.k8s.io/repositories/isv:/kubernetes:/core:/stable:/v1.36/deb  InRelease
Hit:11 http://us.archive.ubuntu.com/ubuntu resolute-backports InRelease     
Fetched 341 kB in 0s (715 kB/s)              
24 packages can be upgraded. Run 'apt list --upgradable' to see them.
```

## Refs

- [HCSEC-2026-33 - HashiCorp Linux Signing GPG Key Update (CA026560)](https://discuss.hashicorp.com/t/hcsec-2026-33-hashicorp-linux-signing-gpg-key-update-ca026560/77734)