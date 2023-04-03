import speech_recognition as sr
import nltk
from nltk.corpus import wordnet as wn
import socket
import struct
import re

betSyn = []
for syn in wn.synsets("bet"):
    for i in syn.lemmas():
        betSyn.append(i.name())
for syn in wn.synsets("do"):
    for i in syn.lemmas():
        betSyn.append(i.name())
for syn in wn.synsets("put"):
    for i in syn.lemmas():
        betSyn.append(i.name())
for syn in wn.synsets("go"):
    for i in syn.lemmas():
        betSyn.append(i.name())
betSynSet=set(betSyn)      
betSyn=list(betSynSet)

underSyn = []
for syn in wn.synsets("under"):
    for i in syn.lemmas():
        underSyn.append(i.name())
for syn in wn.synsets("less"):
    for i in syn.lemmas():
        underSyn.append(i.name())
underSynSet=set(underSyn)
underSyn=list(underSynSet)

overSyn=[]
for syn in wn.synsets("over"):
    for i in syn.lemmas():
        overSyn.append(i.name())
for syn in wn.synsets("more"):
    for i in syn.lemmas():
        overSyn.append(i.name())
overSynSet=set(overSyn)
overSyn=list(overSynSet)

exactSyn=[]
for syn in wn.synsets("exactly"):
    for i in syn.lemmas():
        exactSyn.append(i.name())
exactSynSet=set(exactSyn)
exactSyn=list(exactSynSet)

def getCommand():
    r = sr.Recognizer()

    with sr.Microphone() as source:
        # read the audio data from the default microphone
        audio_data = r.record(source, duration=5)
        print("Recognizing...")
        # convert speech to text
        text = r.recognize_google(audio_data)
    
    text = re.sub('$','',text)
    
    text = re.sub(r'(?<=\d)(,)(?=\d)','',text)
    
    text = re.sub(':',',',text)
    
    print(text)
    
    tokens = nltk.word_tokenize(text)
        
    tagged = nltk.pos_tag(tokens)
    entities = nltk.chunk.ne_chunk(tagged)

    numList = []

    for i,word in enumerate(tokens):
        if word.isdigit():
            numList.append(i)

    betFound = False
    actionFound = False
    bet=-1
    action=-1
            
    for i in numList:
        if tokens[i-1] in exactSyn:
            actionFound=True
            action=1
            continue
        elif tokens[i-1] in underSyn:
            actionFound=True
            action=0
            continue
        elif tokens[i-1] in overSyn:
            actionFound=True
            action=2
            continue
        elif tokens[i-1] in betSyn:
            betFound=True
            bet=int(tokens[i])
            continue
    if not actionFound:
        tokenSet=set(tokens)
        if not tokenSet.isdisjoint(underSynSet):
            actionFound=True
            action=0
        elif not tokenSet.isdisjoint(overSynSet):
            actionFound=True
            action=2
        elif not tokenSet.isdisjoint(exactSynSet):
            actionFound=True
            action=1
        elif '7' in tokens:
            actionFound = True
            action = 1
    print(bet)
    print(action)
    
    return [bet,action]
    
print(betSyn)
print(underSyn)
print(overSyn)
print(exactSyn)

serve = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
serve.bind(('localhost',4149))

serving = True

serve.listen(5)


while serving:
    try:
        (fromUnity,fromAdd) = serve.accept()
        recv = fromUnity.recv(4)
    except socket.timeout:
        serving=False
        serve.close()
        break
    print(int.from_bytes(recv,'little'))
    
    command = getCommand()
    
    toUnity = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    try:
        toUnity.connect(("localhost", 4150))
        toUnity.send(struct.pack("@ii",command[0],command[1]))
        print("sent")
    except socket.timeout:
        serving=False
        toUnity.close()
        serve.close()
        