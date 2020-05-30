# Network Data Ecnryption

## Realization of some network protocol with C#

This protocol work between Server and Client.
The phases of protocol are:

1. Key exchange:

    Client generate client RSA public and private key.
    Server generate server RSA public and private key.
    
    
    Client sends self public key to server.
    Client gets server public key.
    
    
    Server generates AES key.
    Server encrypts AES key with client RSA public key. Server sends it to Client.
    
    
    Client gets encrypted AES key and decrypts it with client RSA private key.
    
    
    Now Client and Server have AES key.
    
    
2. Data exchange:
    
    Client encrypt data with AES. 
    Client signs data with RSA server public key.
    Client calculates encrypted data hash with MD5.
    
    
    Client sends encrypted and signed data to Server
    Client sends hash to Server
    
    
    Server gets encrypted data.
    Server gets hash.
    
    
    Server calculates encrypted data hash.
    Server compares received and calculated hashes.
    If they are not equal Server breaks the operation.
    
    
    Server decrypts data with self RSA private key.
    Server decrypts data with AES.
    
    Data recieved!

