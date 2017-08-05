require 'oauth'
 
CONSUMER_KEY='9a1d16e79a9002f50d4cc76a3b3d8646f834c023'
CONSUMER_SECRET='4779a3dd3dc7f225aa0d1180d3eb9d25b97d27ed'
 
consumer=OAuth::Consumer.new(CONSUMER_KEY, CONSUMER_SECRET,
        :site=>"https://api.zaim.net",
        :request_token_path=>"/v2/auth/request",
        :authorize_url=>"https://auth.zaim.net/users/auth",
        :access_token_path=>"/v2/auth/access")
        
request_token = consumer.get_request_token(:oauth_callback=>"http://google.com")
p request_token.authorize_url
system('open', request_token.authorize_url)
print "Input OAuth Verifier:"
oauth_verifier = gets.chomp.strip
access_token = request_token.get_access_token(:oauth_verifier => oauth_verifier)
p access_token.token
p access_token.secret

#'https://auth.zaim.net/users/auth?oauth_token=JNewYAr3vmMaoHJNmb8vtRVu1GLm3pnE7jTzKtgX6ElSNFRcH3droSaHMypIJYpfDjohi4suoFNdN
#'s1hDmoAmxRkagyYprdr4OFnwjEocDPR4tV3ywPC2tyYjA7MFROG9yukd3MtYEzEz9qmz8iE2g

# "QcNXZSC9Sjd2Y3jPEx6IuY7N5iB9gBZch1jEEbjAo9dAWkwCj0pdZqe5kdFN7VgTH8YrBV2Z8"
# "YlMWUiOUVQCxGPK5DT5YZjnObvTwVtSLZjGu9gvOBgh8HA9gI4H4PmvqtdAouksiB4g"


https://auth.zaim.net/users/auth?oauth_token=pLq4SapunlX3WdjY5aWZ1PhNv4vRUcBW48G7jWrKrQ6RvSUWqh7WKt7yASDKxQXQUeQ
gzINnioKd5S4t5BXKjzSE8wnG5fPkmcuPezuWyHasWakinQMyWe1QYYztAGjWF4XGLUMYDuhfsIm16

QcNXZSC9Sjd2Y3jPEx6IuY7N5iB9gBZch1jEEbjAo9dAWkwCj0pdZqe5kdFN7VgTH8YrBV2Z8
YlMWUiOUVQCxGPK5DT5YZjnObvTwVtSLZjGu9gvOBgh8HA9gI4H4PmvqtdAouksiB4g


https://auth.zaim.net/users/auth?oauth_token=QcNXZSC9Sjd2Y3jPEx6IuY7N5iB9gBZch1jEEbjAo9dAWkwCj0pdZqe5kdFN7VgTH8YrBV2Z8&oauth_token_secret=YlMWUiOUVQCxGPK5DT5YZjnObvTwVtSLZjGu9gvOBgh8HA9gI4H4PmvqtdAouksiB4g