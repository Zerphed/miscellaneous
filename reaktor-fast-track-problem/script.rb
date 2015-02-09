class Node
	attr_reader :value
	attr_accessor :left, :right, :max_value, :next_node

	def initialize(value, left=nil, right=nil)
		@value, @left, @right = value, left, right
		@max_value, @next_node = value, nil
	end

	def isLeaf()
		return (@left == nil and @right == nil)
	end
end

def generateNodes()
	if ARGV.size() != 1
		puts "Invalid number of arguments, use: ruby script.py tree.txt"
		exit()
	end

	begin
		file = File.open(ARGV[0], "r")
		# Generate an array of all the nodes and their values
		nodes = Array.new
		file.each_with_index do |line, idx|
			# Skip the first line which contains the seed
			next if idx == 0
			nodes.concat(line.split(" ").map { |n| Node.new(n.to_i) })
		end
	rescue Exception => msg  
		puts msg
		exit()
	ensure
		file.close() if file != nil
	end

	# Generate the parent->child links for the nodes
	level = offset = 0
	nodes.each_with_index do |node, idx|
		left_idx = idx+(level+1)
		right_idx = left_idx+1
		node.left = nodes[left_idx] if left_idx < nodes.length
		node.right = nodes[right_idx] if right_idx < nodes.length
		if offset == level
			level += 1
			offset = 0
		else
			offset +=1
		end
	end

	return nodes
end

def findMaximumPathSum(nodes)
	cnodes = nodes.dup()
	cnodes.reverse_each do |node|
		next if node.isLeaf()
		l_gt_r = node.left.max_value > node.right.max_value
		node.next_node = l_gt_r ? node.left : node.right
		node.max_value += l_gt_r ? node.left.max_value : node.right.max_value
	end
	return cnodes
end

def main()
	# Generate a tree structure from the argument file and time the operation
	t1 = Time.now()
	nodes = generateNodes()

	# Find the maximum path sum 
	puts "** Searching a tree with #{nodes.length} nodes."
	nodes = findMaximumPathSum(nodes)
	sum = nodes.first.max_value
	t2 = Time.now()
	puts "** Operation finished in #{t2-t1} seconds."
	
	# Print the path and the sum
	node = nodes.first
	while (node != nil) do
		print node.value.to_s()
		print " + " if node.next_node != nil
		node = node.next_node
	end
	print " = #{sum} likes.\n"

end

main()