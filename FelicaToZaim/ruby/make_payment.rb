# -*- coding: utf-8 -*-

require 'oauth'
require 'json'
#require 'uri'
require 'base64'
 
require './ruby/authinfo.rb'

def decode(str)
#	str = URI.escape(str)
#	str = URI.unescape(str)
	str = str.dup
	str = str.encode('UTF-8')
	#	str.force_encoding('UTF-8')
	str = str.scrub('?')
	return str
#	return Base64.decode64(str)
end
 
consumer=OAuth::Consumer.new(CONSUMER_KEY, CONSUMER_SECRET,
                  :site=>"https://api.zaim.net",
                  :request_token_path=>"/v2/auth/request",
                  :authorize_url=>"https://auth.zaim.net/users/auth",
                  :access_token_path=>"/v2/auth/access")
access_token = OAuth::AccessToken.new(consumer, ACCESS_TOKEN, ACCESS_TOKEN_SECRET)


comment = decode(ARGV[5])
name    = decode(ARGV[6])
shopname = decode(ARGV[7])

puts shopname

res = access_token.post('https://api.zaim.net/v2/home/money/payment',
	:mapping=>1,
	:category_id=>ARGV[0],
	:genre_id=>ARGV[1],
	:amount=>ARGV[2],
	:date=>ARGV[3],
	:from_account_id=>ARGV[4],
	:comment=>comment,
	:name=>name,
	:place=>shopname
)

json = JSON.parse(res.body)
pgen = JSON.pretty_generate(json)
puts pgen
