require 'oauth'
require 'json'
 
require './authinfo.rb'
 
consumer=OAuth::Consumer.new(CONSUMER_KEY, CONSUMER_SECRET,
                  :site=>"https://api.zaim.net",
                  :request_token_path=>"/v2/auth/request",
                  :authorize_url=>"https://auth.zaim.net/users/auth",
                  :access_token_path=>"/v2/auth/access")
access_token = OAuth::AccessToken.new(consumer, ACCESS_TOKEN, ACCESS_TOKEN_SECRET)
 
#res = access_token.get('https://api.zaim.net/v2/home/user/verify')

res = access_token.get('https://api.zaim.net/v2/home/category')
json = JSON.parse(res.body)
pgen = JSON.pretty_generate(json)
puts pgen
