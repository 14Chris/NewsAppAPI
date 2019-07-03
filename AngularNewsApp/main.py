import sys
import pyodbc
import pandas as pd
import numpy as np
from sklearn.metrics.pairwise import cosine_similarity
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import linear_kernel
from nltk.corpus import stopwords

idUser = sys.argv[1]

conn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};'
                      'Server=192.168.1.198;'
                      'Database=newsapp;'
                      'UID=sa;'
                      'PWD=azertySQLServer!')

queryUser = 'SELECT a.id,a.title,a.subtitle, a.description FROM ArticleUser au, Article a  WHERE au.id_article = a.id and au.id_user = ' + str(idUser)
dsUser = pd.read_sql(queryUser, con = conn)

queryArticles = 'SELECT id,title,subtitle,description FROM Article'
dsArticles = pd.read_sql(queryArticles, con = conn)

results = {}

def GetRecommendations(idUser):
    tf = TfidfVectorizer(analyzer='word', ngram_range=(1, 3), min_df=0, stop_words=stopwords.words('french'))
    tfidf_matrix = tf.fit_transform(dsArticles['description'])

    cosine_similarities = linear_kernel(tfidf_matrix, tfidf_matrix)

    for idx, row in dsArticles.iterrows():
        similar_indices = cosine_similarities[idx].argsort()[:-100:-1]
        similar_items = [(cosine_similarities[idx][i], dsArticles['id'][i]) for i in similar_indices]

        # First item is the item itself, so remove it.
        # Each dictionary entry is like: [(1,2), (3,4)], with each tuple being (score, item_id)
        results[row['id']] = similar_items[1:]

# hacky little function to get a friendly item name from the description field, given an item ID
def item(id):
    return dsArticles.loc[dsArticles['id'] == id]['description'].tolist()[0].split(' - ')[0]

# Just reads the results out of the dictionary. No real logic here.
def recommend(num):
    
    print("[")       
# print("Recommending " + str(num) + " articles similar to " + item(item_id) + "...")
    for idArticle in dsUser['id']:
        item_id = idArticle
        recs = results[item_id][:num]

        print("[")

        for rec in recs:
                print("{\"idArticle\":\"" + str(rec[1]) + "\",\"concordance\":\""+str(rec[0])+"\"}")
                print(",")

        print("]")
        print(",")
        
    print("]")    

GetRecommendations(idUser)

recommend(num=20)
