using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVPluggin
{
	public struct CalibrationResult
	{
		public CalibrationResult(bool result, Object data)
		{
			Result = result;
			Data = data;
		}
		public bool Result;
		public Object Data;
	}
	public struct CheckBoardDefinition
	{
		public CheckBoardDefinition(Size pattern, float squaredWidth, float patternDistance)
		{
			Pattern =pattern;
			SquaredWidth=squaredWidth;
			PatternDistance=patternDistance;
		}

		public Size Pattern;
		public float SquaredWidth;
		public float PatternDistance;
	}
	public abstract class Calibration
	{
		/*
			Performs calibration processes:
				- Multithread calibration
		*/

		int? Driver;
	
		protected MCvTermCriteria Criteria;
		protected IntrinsicCameraParameters CameraIntrinsic;
		protected bool UseDistortion {get;set;}
		CheckBoardDefinition _CheckBoard;
		protected CheckBoardDefinition CheckBoard
		{
			get {return _CheckBoard;}
			set {
				_CheckBoard=value;
				
				ObjectPoints = new MCvPoint3D32f[_CheckBoard.Pattern.Width][];
				for(int x=0;x<_CheckBoard.Pattern.Width;x++)
				{
					ObjectPoints[x] = new MCvPoint3D32f[_CheckBoard.Pattern.Height];
					for(int y=0;y<_CheckBoard.Pattern.Height;y++)
					{
						ObjectPoints[x][y] = new MCvPoint3D32f(x*_CheckBoard.SquaredWidth,y*_CheckBoard.SquaredWidth,0f);
					}
				}
			}
		}

		
		protected	MCvPoint3D32f[][] ObjectPoints;
		protected PointF[][] ImagePoint;


		protected virtual CalibrationResult Process() { return new CalibrationResult(false, null); }
		protected virtual void Before() { }
		protected virtual void After() { }

		public Calibration()
		{
			Driver = 0/*Driver.Instance()*/;
			Criteria = new MCvTermCriteria(100, 0.001);
		}

		public virtual void Start()
		{
			Before();
			Process();
			After();
		}

	}
public class CameraIntrinsics: Calibration
{
	/*
		Camera calibration algorithms, based on [Zhang2000] and [BouguetMCT]:
			- Camera matrix
			- Distortion vector
	*/

	public CameraIntrinsics()
		:base()
	{
	}
	protected override CalibrationResult Process()
	{
		ExtrinsicCameraParameters[] ext = new ExtrinsicCameraParameters[5];
		CameraCalibration.CalibrateCamera(this.ObjectPoints, this.ImagePoint, this.CheckBoard.Pattern, this.CameraIntrinsic, Emgu.CV.CvEnum.CalibType.Default, this.Criteria, out ext);
		return new CalibrationResult(false,null);
	/*

	def _start(this, progressCallback, afterCallback):
		ret, mtx, dist, rvecs, tvecs = cv2.calibrateCamera(this.objPointsStack, this.imagePointsStack, this.shape)
		
		if progressCallback is not null:
			progressCallback(100)

		if ret:
			response = (true, (mtx, dist[0], rvecs, tvecs))
		else:
			response = (false, Error.CalibrationError)

		if afterCallback is not null:
			afterCallback(response)
*/
	}

	public CalibrationResult DetectCheckBoard()
	{
		/*
	def detectChessboard(this, frame, capture=false):
		if this.patternRows < 2 or this.patternColumns < 2:
			return false, frame

		gray = cv2.cvtColor(frame,cv2.COLOR_BGR2GRAY)
		this.shape = gray.shape
		retval, corners = cv2.findChessboardCorners(gray, (this.patternColumns,this.patternRows), flags=cv2.CALIB_CB_FAST_CHECK)
		if retval:
			cv2.cornerSubPix(gray, corners, winSize=(11,11), zeroZone=(-1,-1), criteria=this.criteria)
			if capture:
				if len(this.objPointsStack) < 12:
					this.imagePointsStack.append(corners)
					this.objPointsStack.append(this.objpoints) 
			cv2.drawChessboardCorners(frame, (this.patternColumns,this.patternRows), corners, retval)
		return retval, frame
		 * */
		return new CalibrationResult(false,null);
	}

	public void ClearImageStack()
	{

	}
}

public class LaserTriangulation : Calibration
{
	/*
	Laser triangulation algorithms:
		- Laser coordinates matrix
		- Pattern's origin
		- Pattern's normal
	*/
	Bitmap Image { get; set; }
	public LaserTriangulation()
	{
		///TODO GetLASER INFO
	}

	protected override CalibrationResult Process()
	{
		return new CalibrationResult(false, null);
		/*
def _start(this, progressCallback, afterCallback):
	XL = null
	XR = null

	if os.name=='nt':
		flush = 2
	else:
		flush = 1

	if this.driver.isConnected:

		board = this.driver.board
		camera = this.driver.camera

		##-- Switch off lasers
		board.setLeftLaserOff()
		board.setRightLaserOff()

		##-- Setup motor
		step = 5
		angle = 0
		board.setSpeedMotor(1)
		board.enableMotor()
		board.setSpeedMotor(150)
		board.setAccelerationMotor(150)
		time.sleep(0.2)

		if progressCallback is not null:
			progressCallback(0)

		while this.isCalibrating and abs(angle) < 180:

			if progressCallback is not null:
				progressCallback(1.11*abs(angle/2.))

			angle += step

			camera.setExposure(profile.getProfileSettingNumpy('exposure_calibration'))

			#-- Image acquisition
			imageRaw = camera.captureImage(flush=true, flushValue=flush)

			#-- Pattern detection
			ret = this.getPatternPlane(imageRaw)

			if ret is not null:
				step = 4 #2

				d, n, corners = ret

				camera.setExposure(profile.getProfileSettingNumpy('exposure_calibration')/2.)
			
				#-- Image laser acquisition
				imageRawLeft = camera.captureImage(flush=true, flushValue=flush)
				board.setLeftLaserOn()
				imageLeft = camera.captureImage(flush=true, flushValue=flush)
				board.setLeftLaserOff()
				this.image = imageLeft
				if imageLeft is null:
					break
					
				imageRawRight = camera.captureImage(flush=true, flushValue=flush)
				board.setRightLaserOn()
				imageRight = camera.captureImage(flush=true, flushValue=flush)
				board.setRightLaserOff()
				this.image = imageRight
				if imageRight is null:
					break

				#-- Pattern ROI mask
				imageRaw = this.cornersMask(imageRaw, corners)
				imageLeft = this.cornersMask(imageLeft, corners)
				imageRawLeft = this.cornersMask(imageRawLeft, corners)
				imageRight = this.cornersMask(imageRight, corners)
				imageRawRight = this.cornersMask(imageRawRight, corners)

				#-- Line segmentation
				uL, vL = this.getLaserLine(imageLeft, imageRawLeft)
				uR, vR = this.getLaserLine(imageRight, imageRawRight)

				#-- Point Cloud generation
				xL = this.getPointCloudLaser(uL, vL, d, n)
				if xL is not null:
					if XL is null:
						XL = xL
					else:
						XL = np.concatenate((XL,xL))
				xR = this.getPointCloudLaser(uR, vR, d, n)
				if xR is not null:
					if XR is null:
						XR = xR
					else:
						XR = np.concatenate((XR,xR))
			else:
				step = 5
				this.image = imageRaw

			board.setRelativePosition(step)
			board.moveMotor()
			time.sleep(0.1)

		# this.saveScene('XL.ply', XL)
		# this.saveScene('XR.ply', XR)

		#-- Compute planes
		dL, nL, stdL = this.computePlane(XL, 'l')
		dR, nR, stdR = this.computePlane(XR, 'r')

	##-- Switch off lasers
	board.setLeftLaserOff()
	board.setRightLaserOff()

	#-- Disable motor
	board.disableMotor()

	#-- Restore camera exposure
	camera.setExposure(profile.getProfileSettingNumpy('exposure_calibration'))

	if this.isCalibrating and nL is not null and nR is not null:
		response = (true, ((dL, nL, stdL), (dR, nR, stdR)))
		if progressCallback is not null:
			progressCallback(100)
	else:
		if this.isCalibrating:
			response = (false, Error.CalibrationError)
		else:
			response = (false, Error.CalibrationCanceled)

	this.image = null

	if afterCallback is not null:
		afterCallback(response)
		 */
	}
	public void GetPatternPlane(Bitmap image)
	{
		CalibrationResult ret = SolvePnP(image,this.ObjectPoints, this.CameraIntrinsic, this.CheckBoard);
		if(ret!=null && ret.Result)
		{

		}
		/*
def getPatternPlane(this, image):
	if image is not null:
		ret = this.solvePnp(image, this.objpoints, this.cameraMatrix, this.distortionVector, this.patternColumns, this.patternRows)
		if ret is not null:
			if ret[0]:
				R = ret[1]
				t = ret[2].T[0]
				n = R.T[2]
				c = ret[3]
				d = -np.dot(n,t)
				return (d, n, c)
		 */
	}
	public void GetLaserLine()
	{
		/*
def getLaserLine(this, imageLaser, imageRaw):
	#-- Image segmentation
	sub = cv2.subtract(imageLaser,imageRaw)
	r,g,b = cv2.split(sub)

	#-- Threshold
	r = cv2.threshold(r, this.threshold, 255.0, cv2.THRESH_TOZERO)[1]

	h, w = r.shape

	#-- Peak detection: center of mass
	W = np.array((np.matrix(np.linspace(0,w-1,w)).T*np.matrix(np.ones(h))).T)
	s = r.sum(axis=1)
	v = np.where(s > 0)[0]
	u = (W*r).sum(axis=1)[v] / s[v]

	return u, v
		 */
	}
	public void GetPointCloudLaser()
	{
		/*
def getPointCloudLaser(this, u, v, d, n):
	fx = this.cameraMatrix[0][0]
	fy = this.cameraMatrix[1][1]
	cx = this.cameraMatrix[0][2]
	cy = this.cameraMatrix[1][2]

	x = np.concatenate(((u-cx)/fx, (v-cy)/fy, np.ones(len(u)))).reshape(3,len(u))

	X = -d/np.dot(n,x)*x

	return X.T
		 */
	}
	public void ComputePlane()
	{
		/*
def computePlane(this, X, side):
	if X is not null:
		X = np.matrix(X).T
		n = X.shape[1]
		std=0
		if n > 3:
			final_points=[]

			for trials in range(30):
				X=np.matrix(X)
				n=X.shape[1]

				Xm = X.sum(axis=1)/n
				M = np.array(X-Xm)
				#begin = datetime.datetime.now()
				U = linalg.svds(M, k=2)[0]
				#print "nº {0}  time {1}".format(n, datetime.datetime.now()-begin)
				s, t = U.T
				n = np.cross(s, t)
				if n[2] < 0:
					n *= -1
				d = np.dot(n,np.array(Xm))[0]
				distance_vector=np.dot(M.T,n)

				#If last std is equal to current std, break loop
				if std==distance_vector.std():
					break

				std = distance_vector.std()

				final_points=np.where(abs(distance_vector)<abs(2*std) )[0]
				#print 'iteration ', trials, 'd,n,std, len(final_points)', d,n,std, len(final_points)

				X=X[:, final_points]

				#Save each iteration point cloud
				# if side == 'l':
				# 	this.saveScene('new_'+str(trials)+'_XL.ply', np.asarray(X.T))
				# else:
				# 	this.saveScene('new_'+str(trials)+'_XR.ply', np.asarray(X.T))

				if std<0.1 or len(final_points)<1000:
					break

			return d, n, std
		else:
			return null, null, null
	else:
		return null, null, null
		 */
	}

	public void CornerMask()
	{
		/*
def cornersMask(this, frame, corners):
	p1 = corners[0][0]
	p2 = corners[this.patternColumns-1][0]
	p3 = corners[this.patternColumns*(this.patternRows-1)-1][0]
	p4 = corners[this.patternColumns*this.patternRows-1][0]
	p11 = min(p1[1], p2[1], p3[1], p4[1])
	p12 = max(p1[1], p2[1], p3[1], p4[1])
	p21 = min(p1[0], p2[0], p3[0], p4[0])
	p22 = max(p1[0], p2[0], p3[0], p4[0])
	d = max(corners[1][0][0]-corners[0][0][0],
			corners[1][0][1]-corners[0][0][1],
			corners[this.patternColumns][0][1]-corners[0][0][1],
			corners[this.patternColumns][0][0]-corners[0][0][0])
	mask = np.zeros(frame.shape[:2], np.uint8)
	mask[p11-d:p12+d,p21-d:p22+d] = 255
	frame = cv2.bitwise_and(frame, frame, mask=mask)
	return frame
		 */
	}
	public void SaveScene()
	{
		/*
def saveScene(this, filename, pointCloud):
	if pointCloud is not null:
		f = open(filename, 'wb')
		this.saveSceneStream(f, pointCloud)
		f.close()

def saveSceneStream(this, stream, pointCloud):
	frame  = "ply\n"
	frame += "format binary_little_endian 1.0\n"
	frame += "comment Generated by Horus software\n"
	frame += "element vertex {0}\n".format(len(pointCloud))
	frame += "property float x\n"
	frame += "property float y\n"
	frame += "property float z\n"
	frame += "property uchar red\n"
	frame += "property uchar green\n"
	frame += "property uchar blue\n"
	frame += "element face 0\n"
	frame += "property list uchar int vertex_indices\n"
	frame += "end_header\n"
	for point in pointCloud:
		frame += struct.pack("<fffBBB", point[0], point[1], point[2] , 255, 0, 0)
	stream.write(frame)
		 */
	}

	public CalibrationResult SolvePnP(Bitmap image, MCvPoint3D32f[] objpoints, IntrinsicCameraParameters camIntrinsic, CheckBoardDefinition pattern)
	{
		//cvCvtColor(image, cv2.COLOR_BGR2GRAY)
		Image<Gray, Byte> gray = new Image<Gray, byte>(image);

		// the fast check flag reduces significantly the computation time if the pattern is out of sight 
		PointF[] corners = CameraCalibration.FindChessboardCorners(gray, pattern.Pattern, Emgu.CV.CvEnum.CalibCbType.FastCheck);
		ExtrinsicCameraParameters ret = null;
		if (corners != null)
			ret = CameraCalibration.SolvePnP(objpoints, corners, camIntrinsic);
		return new CalibrationResult(ret != null, ret);
	}
}
	public class SimpleLaserTriangulation : Calibration
	{

		/*
			Laser triangulation algorithms:
				- Laser coordinates matrix
				- Pattern's origin
				- Pattern's normal
		*/
		public SimpleLaserTriangulation()
			: base()
		{
		}
		protected override CalibrationResult Process()
		{
			return new CalibrationResult(false, null);
			/*
		t = null

		if this.driver.isConnected:

			board = this.driver.board
			camera = this.driver.camera

			##-- Switch off lasers
			board.setLeftLaserOff()
			board.setRightLaserOff()

			##-- Move pattern until ||(R-I)|| < e
			board.setSpeedMotor(1)
			board.enableMotor()
			time.sleep(0.3)

			t, n, corners = this.getPatternDepth(board, camera, progressCallback)

			if t is not null and corners is not null:
				time.sleep(0.1)

				#-- Get images
				imgRaw = camera.captureImage(flush=true, flushValue=1)
				board.setLeftLaserOn()
				imgLasL = camera.captureImage(flush=true, flushValue=1)
				board.setLeftLaserOff()
				board.setRightLaserOn()
				imgLasR = camera.captureImage(flush=true, flushValue=1)
				board.setRightLaserOff()

				if imgRaw is not null and imgLasL is not null and imgLasR is not null:
					##-- Corners ROI mask
					imgLasL = this.cornersMask(imgLasL, corners)
					imgLasR = this.cornersMask(imgLasR, corners)

					##-- Obtain Left Laser Line
					retL = this.obtainLine(imgRaw, imgLasL)

					##-- Obtain Right Laser Line
					retR = this.obtainLine(imgRaw, imgLasR)

			#-- Disable motor
			board.disableMotor()

		if this.isCalibrating and t is not null and not (0 in retL[0] or 0 in retR[0]):
			response = (true, ([t, n], [retL[0], retR[0]], [retL[1], retR[1]]))
			if progressCallback is not null:
				progressCallback(100)
		else:
			if this.isCalibrating:
				response = (false, Error.CalibrationError)
			else:
				response = (false, Error.CalibrationCanceled)

		this.image = null

		if afterCallback is not null:
			afterCallback(response)
			 */
		}
		public void GetPatternDepth()
		{
			/*
	def getPatternDepth(this, board, camera, progressCallback):
		epsilon = 0.05
		distance = np.inf
		distanceAnt = np.inf
		angle = 30
		t = null
		n = null
		corners = null
		tries = 5
		board.setRelativePosition(angle)
		board.setSpeedMotor(150)
		board.setAccelerationMotor(300)

		if progressCallback is not null:
			progressCallback(0)

		while this.isCalibrating and distance > epsilon and tries > 0:
			image = camera.captureImage(flush=true, flushValue=1)
			if image is not null:
				ret = this.solvePnp(image, this.objpoints, this.cameraMatrix, this.distortionVector, this.patternColumns, this.patternRows)
				if ret is not null:
					if ret[0]:
						R = ret[1]
						t = ret[2].T[0]
						n = R.T[2]
						corners = ret[3]
						distance = np.linalg.norm((0,0,1)-n)
						if distance < epsilon or distanceAnt < distance:
							if this.isCalibrating:
								board.setRelativePosition(-angle)
								board.moveMotor()
							break
						distanceAnt = distance
						angle = np.max(((distance-epsilon) * 30, 5))
				else:
					tries -= 1
			else:
				tries -= 1
			if this.isCalibrating:
				board.setRelativePosition(angle)
				board.moveMotor()

			if progressCallback is not null:
				if distance < np.inf:
					progressCallback(min(80,max(0,80-100*abs(distance-epsilon))))

		if this.isCalibrating:
			image = camera.captureImage(flush=true, flushValue=1)
			if image is not null:
				ret = this.solvePnp(image, this.objpoints, this.cameraMatrix, this.distortionVector, this.patternColumns, this.patternRows)
				if ret is not null:
					R = ret[1]
					t = ret[2].T[0]
					n = R.T[2]
					corners = ret[3]
					distance = np.linalg.norm((0,0,1)-n)
					angle = np.max(((distance-epsilon) * 30, 5))

					if progressCallback is not null:
						progressCallback(90)

		#print "Distance: {0} Angle: {1}".format(round(distance,3), round(angle,3))

		return t, n, corners
			 */

		}
		public void GetCornerMask()
		{
			/*
	def cornersMask(this, frame, corners):
		p1 = corners[0][0]
		p2 = corners[this.patternColumns-1][0]
		p3 = corners[this.patternColumns*(this.patternRows-1)-1][0]
		p4 = corners[this.patternColumns*this.patternRows-1][0]
		p11 = min(p1[1], p2[1], p3[1], p4[1])
		p12 = max(p1[1], p2[1], p3[1], p4[1])
		p21 = min(p1[0], p2[0], p3[0], p4[0])
		p22 = max(p1[0], p2[0], p3[0], p4[0])
		d = max(corners[1][0][0]-corners[0][0][0],
				corners[1][0][1]-corners[0][0][1],
				corners[this.patternColumns][0][1]-corners[0][0][1],
				corners[this.patternColumns][0][0]-corners[0][0][0])
		mask = np.zeros(frame.shape[:2], np.uint8)
		mask[p11-d:p12+d,p21-d:p22+d] = 255
		frame = cv2.bitwise_and(frame, frame, mask=mask)
		return frame
			 * */
		}
		public void ObtainLine()
		{
			/*
	def obtainLine(this, imgRaw, imgLas):
		u1 = u2 = 0

		height, width, depth = imgRaw.shape
		imgLine = np.zeros((height,width,depth), np.uint8)

		diff = cv2.subtract(imgLas, imgRaw)
		r,g,b = cv2.split(diff)
		kernel = cv2.getStructuringElement(cv2.MORPH_RECT,(3,3))
		r = cv2.morphologyEx(r, cv2.MORPH_OPEN, kernel)
		imgGray = cv2.merge((r,r,r))
		edges = cv2.threshold(r, 20.0, 255.0, cv2.THRESH_BINARY)[1]
		edges3 = cv2.merge((edges,edges,edges))
		lines = cv2.HoughLines(edges, 1, np.pi/180, 200)

		if lines is not null:
			rho, theta = lines[0][0]
			#-- Calculate coordinates
			u1 = rho / np.cos(theta)
			u2 = u1 - height * np.tan(theta)

			#-- Draw line
			cv2.line(imgLine,(int(round(u1)),0),(int(round(u2)),height-1),(255,0,0),5)

		return [[u1, u2], [imgLas, imgGray, edges3, imgLine]]
			 */
		}
		public void SolvePnp()
		{
			/*
	def solvePnp(this, image, objpoints, cameraMatrix, distortionVector, patternColumns, patternRows):
		gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
		# the fast check flag reduces significantly the computation time if the pattern is out of sight 
		retval, corners = cv2.findChessboardCorners(gray, (patternColumns,patternRows), flags=cv2.CALIB_CB_FAST_CHECK)

		if retval:
			cv2.cornerSubPix(gray, corners, winSize=(11,11), zeroZone=(-1,-1), criteria=this.criteria)
			if this.useDistortion:
				ret, rvecs, tvecs = cv2.solvePnP(objpoints, corners, cameraMatrix, distortionVector)
			else:
				ret, rvecs, tvecs = cv2.solvePnP(objpoints, corners, cameraMatrix, null)
			return (ret, cv2.Rodrigues(rvecs)[0], tvecs, corners)
			 */
		}
	}

	public class PlatformExtrinsics : Calibration
	{
		/*
			Platform extrinsics algorithms:
				- Rotation matrix
				- Translation vector
		*/
		public PlatformExtrinsics()
			: base()
		{

		}

		public Bitmap Image { get; set; }
		float ExtrinsicsStep { get; set; }

		protected override CalibrationResult Process()
		{
			return new CalibrationResult(false, null);
			/*
	def _start(this, progressCallback, afterCallback):
		t = null

		if this.driver.isConnected:

			board = this.driver.board
			camera = this.driver.camera

			x = []
			y = []
			z = []

			##-- Switch off lasers
			board.setLeftLaserOff()
			board.setRightLaserOff()

			##-- Move pattern 180 degrees
			step = this.extrinsicsStep # degrees
			angle = 0
			board.setSpeedMotor(1)
			board.enableMotor()
			board.setSpeedMotor(150)
			board.setAccelerationMotor(200)
			time.sleep(0.2)

			if progressCallback is not null:
				progressCallback(0)

			while this.isCalibrating and abs(angle) < 180:
				angle += step
				t = this.getPatternPosition(step, board, camera)
				if progressCallback is not null:
					progressCallback(1.1*abs(angle/2.))
				time.sleep(0.1)
				if t is not null:
					x += [t[0][0]]
					y += [t[1][0]]
					z += [t[2][0]]

			x = np.array(x)
			y = np.array(y)
			z = np.array(z)

			points = zip(x,y,z)

			if len(points) > 4:

				#-- Fitting a plane
				point, normal = this.fitPlane(points)

				if normal[1] > 0:
					normal = -normal

				#-- Fitting a circle inside the plane
				center, R, circle = this.fitCircle(point, normal, points)

				# Get real origin
				t = center - this.patternDistance * np.array(normal)

			#-- Disable motor
			board.disableMotor()

		if this.isCalibrating and t is not null and np.linalg.norm(t-[5,80,320]) < 100:
			response = (true, (R, t, center, point, normal, [x,y,z], circle))
			if progressCallback is not null:
				progressCallback(100)
		else:
			if this.isCalibrating:
				response = (false, Error.CalibrationError)
			else:
				response = (false, Error.CalibrationCanceled)

		this.image = null

		if afterCallback is not null:
			afterCallback(response)
			 */
		}

		public void GetPatternPosition()
		{
			/*
				def getPatternPosition(this, step, board, camera):
					t = null
					image = camera.captureImage(flush=true, flushValue=1)
					if image is not null:
						this.image = image
						ret = this.solvePnp(image, this.objpoints, this.cameraMatrix, this.distortionVector, this.patternColumns, this.patternRows)
						if ret is not null:
							if ret[0]:
								t = ret[2]
						board.setRelativePosition(step)
						board.moveMotor()
					return t
			 */
		}

		public void SolvePnp()
		{
			/*
				def solvePnp(this, image, objpoints, cameraMatrix, distortionVector, patternColumns, patternRows):
					gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
					# the fast check flag reduces significantly the computation time if the pattern is out of sight 
					retval, corners = cv2.findChessboardCorners(gray, (patternColumns,patternRows), flags=cv2.CALIB_CB_FAST_CHECK)

					if retval:
						cv2.cornerSubPix(gray, corners, winSize=(11,11), zeroZone=(-1,-1), criteria=this.criteria)
						if this.useDistortion:
							ret, rvecs, tvecs = cv2.solvePnP(objpoints, corners, cameraMatrix, distortionVector)
						else:
							ret, rvecs, tvecs = cv2.solvePnP(objpoints, corners, cameraMatrix, null)
						return (ret, cv2.Rodrigues(rvecs)[0], tvecs, corners)
			 */
		}
		public void DistanceToPlane()
		{
			/*
			#-- Fitting a plane
			def distanceToPlane(this, p0,n0,p):
				return np.dot(np.array(n0),np.array(p)-np.array(p0))    
			 */
		}
		public void ResidualsPlane()
		{
			/*
			def residualsPlane(this, parameters,dataPoint):
				px,py,pz,theta,phi = parameters
				nx,ny,nz = np.sin(theta)*np.cos(phi),np.sin(theta)*np.sin(phi),np.cos(theta)
				distances = [this.distanceToPlane([px,py,pz],[nx,ny,nz],[x,y,z]) for x,y,z in dataPoint]
				return distances
			 */
		}
		public void FitPlane()
		{
			/*
			def fitPlane(this, data):
				estimate = [0, 0, 0, 0, 0] # px,py,pz and zeta, phi
				#you may automize this by using the center of mass data
				# note that the normal vector is given in polar coordinates
				bestFitValues, ier = optimize.leastsq(this.residualsPlane, estimate, args=(data))
				xF,yF,zF,tF,pF = bestFitValues

				#this.point  = [xF,yF,zF]
				this.point = data[0]
				this.normal = -np.array([np.sin(tF)*np.cos(pF),np.sin(tF)*np.sin(pF),np.cos(tF)])

				return this.point, this.normal
			 */
		}
		public void ResidualCircle()
		{
			/*
	def residualsCircle(this, parameters, dataPoint):
		r,s,Ri = parameters
		planePoint = s*this.s + r*this.r + np.array(this.point)
		distance = [ np.linalg.norm( planePoint-np.array([x,y,z])) for x,y,z in dataPoint]
		res = [(Ri-dist) for dist in distance]
		return res
			 */
		}
		public void FitCircle()
		{
			/*
	def fitCircle(this, point, normal, data):
		#creating two inplane vectors
		this.s = np.cross(np.array([1,0,0]),np.array(normal))#assuming that normal not parallel x!
		this.s = this.s/np.linalg.norm(this.s)
		this.r = np.cross(np.array(normal),this.s)
		this.r = this.r/np.linalg.norm(this.r)#should be normalized already, but anyhow

		# Define rotation
		R = np.array([this.s,this.r,normal]).T

		estimateCircle = [0, 0, 0] # px,py,pz and zeta, phi
		bestCircleFitValues, ier = optimize.leastsq(this.residualsCircle, estimateCircle, args=(data))

		rF,sF,RiF = bestCircleFitValues

		# Synthetic Data
		centerPoint = sF*this.s + rF*this.r + np.array(this.point)
		synthetic = [list(centerPoint+ RiF*np.cos(phi)*this.r+RiF*np.sin(phi)*this.s) for phi in np.linspace(0, 2*np.pi,50)]
		[cxTupel,cyTupel,czTupel] = [ x for x in zip(*synthetic)]

		return centerPoint, R, [cxTupel,cyTupel,czTupel]
			 */
		}
	}

}