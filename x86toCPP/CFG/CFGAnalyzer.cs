namespace x86toCPP;

/// <summary>
/// Visitor pattern implementation to visit the parse tree.
/// Builds a Control Flow Graph given an appropriately validated parse tree.
/// </summary>
public class CFGAnalyzer
{
	// class for easily managing loop information during loop detection
	private class LoopInfo {
		public bool LoopDetected { get; set; } = false; 				// set loop_detected = false
		public CFGNode? LoopHeader { get; set; } = null;				// set loop_header = null
		public CFGNode? LoopBody { get; set; } = null;					// set loop_body = null
		public CFGNode? LoopExit { get; set; } = null;					// set loop_exit = null
		public string? CounterRegister { get; set; } = null;		// set counter_reg = null
		public CFGNode? InitBlock { get; set; } = null;					// set init_block = null
		public CFGNode? IncrementBlock { get; set; } = null;		// set increment_block = null
	}

	private List<CFGNode> nodes;

	public CFGAnalyzer(List<CFGNode> nodes) {
		this.nodes = nodes;
	}

	private LoopInfo DetectLoop() {
		LoopInfo res = new LoopInfo();

		// Step 1: Identify potential loop headers (nodes with back-edges)
			// for each node in cfg.nodes:
			// for each predecessor in node.predecessors:
			//     if predecessor is dominated_by node and predecessor in node.successors:
			//         // Back-edge detected (predecessor -> node)
			//         set loop_header = node
			//         set back_edge_source = predecessor
			//         break
			// if loop_header not null:
			//     break
		foreach(CFGNode node in nodes) {
			// TODO: handle predecessors?
			if(res.LoopHeader != null) break;
		}

		// Step 2: Validate loop header (contains condition check, e.g., cmp + conditional jump)
			// if loop_header not null:
			// for each instruction in loop_header.instructions:
			//     if instruction matches "cmp [register], [value/register]":
			//         set counter_reg = instruction.register
			//         set condition_instruction = instruction
			//         break
			// if condition_instruction not null:
			//     for each successor in loop_header.successors:
			//         if successor NOT in loop (based on back-edge analysis):
			//             set loop_exit = successor
			//         else:
			//             set loop_body = successor
		if(res.LoopHeader != null) {
			foreach(ASTNode node in res.LoopHeader.ParseNodes) {
				// TODO: validate ASTNode as an instruction
			}
		}

		// Step 3: Find initialization block (before loop header, sets counter_reg)
			// for each predecessor in loop_header.predecessors:
			// if predecessor not in loop:
			//     for each instruction in predecessor.instructions:
			//         if instruction matches "mov [counter_reg], [immediate]" or 
			//            instruction matches "xor [counter_reg], [counter_reg]":
			//             set init_block = predecessor
			//             break
			//     if init_block not null:
			//         break
		// TODO: predecessor again... T_T

		// Step 4: Find increment/decrement block (within loop, modifies counter_reg)
			// for each node in loop (reachable from loop_body, including back_edge_source):
			// for each instruction in node.instructions:
			//     if instruction matches "inc [counter_reg]" or "dec [counter_reg]" or 
			//        instruction matches "add [counter_reg], [immediate]" or 
			//        instruction matches "sub [counter_reg], [immediate]":
			//         set increment_block = node
			//         break
			// if increment_block not null:
			//     break
		

		// Step 5: Validate for-loop structure
			// if loop_header not null and init_block not null and 
			// 	loop_body not null and increment_block not null and 
			// 	loop_exit not null and counter_reg not null:
			// 		set loop_detected = true
			// 		return true, loop_header, init_block, loop_body, increment_block, loop_exit, counter_reg
			// else:
			// 		return false, null, null, null, null, null, null
		// hmmm...

		return res;
	}

	// ----- utilities ----
	/// <summary>
	/// Checks whether a path exists from the `start` node to the `end` node. Calls `DFS(...)`
	/// </summary>
	/// <returns>True if a non-cyclic path exists from start to end; otherwise, false.</returns>
	public bool PathExists(CFGNode start, CFGNode end) {
		// don't detect cycles here; strictly topological
		// loop-finding is handled in the CFGAnalyzer itself
		HashSet<CFGNode> visited = new HashSet<CFGNode>();
		return DFS(start, end, visited);
	}

	/// <summary>
	/// Recursive implementation of DFS for the Control Flow Graph (`CFG`) implementation of `x86toCPP`.
	/// </summary>
	/// <returns>True if a non-cyclic path exists from `current` to `target`; otherwise, false.</returns>
	private bool DFS(CFGNode current, CFGNode target, HashSet<CFGNode> visited) {
		// start and end are equal
		if(current == target) return true;
		// prevent cycles for DFS;
		// the loop detection is handled in the execution of the CFGAnalyzer.
		if (!visited.Add(current)) return false;
		foreach(CFGNode edge in current.Edges) {
			// if DFS returns true, then a non-cyclic path exists
			if(DFS(edge, target, visited)) return true;
		}
		return false;
	}
}
